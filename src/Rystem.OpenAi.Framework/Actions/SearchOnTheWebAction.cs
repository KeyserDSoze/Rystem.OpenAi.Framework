using System.Text;
using Rystem.OpenAi.Chat;

namespace Rystem.OpenAi.Framework
{
    internal sealed class SearchOnTheWebAction : IOpenAiAction
    {
        private readonly IWebSearchService _webSearchService;
        public string Id => "2";
        public string Description => "perform a search on the web and insert here a description about what to search";
        public SearchOnTheWebAction(IWebSearchService webSearchService)
        {
            _webSearchService = webSearchService;
        }
        private const string CompleteRequest = "Search in the previous message information related to this task: {0} for the main request {1}. If you don't find anything write only the word '''false''' otherwise only the word '''true'''.";
        private const int MaxLenghtForRequest = 10_000;
        public async ValueTask<ChatMessage[]> ExecuteAsync(IOpenAi openAi, OpenAiThought thought, string startingRequest, string request, AgentStatus status, CancellationToken cancellationToken = default)
        {
            StringBuilder stringBuilder = new();
            await foreach (var search in _webSearchService.SearchAsync($"{thought.Title} {request}", cancellationToken))
            {
                var splitFor = search.Content.Length / 10000 + 1;
                for (var i = 0; i < splitFor; i++)
                {
                    var start = i * MaxLenghtForRequest;
                    var end = (i + 1) * MaxLenghtForRequest;
                    if (end > search.Content.Length - 1)
                        end = search.Content.Length - 1;
                    var content = search.Content[start..end];
                    var response = await Try.WithDefaultOnCatchValueTaskAsync(() =>
                        openAi.Chat
                        .RequestWithSystemMessage(content)
                        .AddUserMessage(string.Format(CompleteRequest, request, startingRequest))
                        .ExecuteAndCalculateCostAsync()).NoContext();
                    if (response.Exception == null)
                    {
                        status.Cost += response.Entity.CalculateCost();
                        var messageFromResponse = response.Entity.Result.Choices[0].Message.Content;
                        if (!messageFromResponse.ToLower().StartsWith("false"))
                            stringBuilder.AppendLine(content);
                    }
                }
                if (stringBuilder.Length > MaxLenghtForRequest)
                    break;
            }
            return new ChatMessage[1] {
                new ChatMessage{
                    Content = stringBuilder.ToString(),
                    Role = ChatRole.System
                }
            };
        }
    }
}
