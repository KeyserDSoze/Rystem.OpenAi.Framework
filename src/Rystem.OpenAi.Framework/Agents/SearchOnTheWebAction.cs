using System.Text;
using Rystem.OpenAi.Chat;

namespace Rystem.OpenAi.Framework
{
    internal sealed class SearchOnTheWebAction : IOpenAiAction
    {
        private readonly IWebSearchService _webSearchService;
        private readonly IOpenAiUtility _utility;
        public string Id => "2";
        public string Description => "perform a search on the web and insert here a description about what to search";
        public SearchOnTheWebAction(IWebSearchService webSearchService, IOpenAiUtility utility)
        {
            _webSearchService = webSearchService;
            _utility = utility;
        }
        public async ValueTask<ChatMessage[]> ExecuteAsync(IOpenAi openAi, string request, AgentStatus status, CancellationToken cancellationToken = default)
        {
            StringBuilder stringBuilder = new();
            await foreach (var search in _webSearchService.SearchAsync(request, cancellationToken))
            {
                var splitFor = search.Content.Length / 10000;
                for (var i = 0; i < splitFor; i++)
                {
                    var start = i * 10000;
                    var end = (i + 1) * 10000;
                    if (end > search.Content.Length - 1)
                        end = search.Content.Length - 1;
                    var content = search.Content[start..end];
                    var response = await Try.WithDefaultOnCatchValueTaskAsync(() => openAi.Chat.RequestWithSystemMessage($"Search in the next message information to answer to: {request}. If you don't find anything respond with word 'false'.")
                        .AddUserMessage(content)
                        .ExecuteAndCalculateCostAsync()).NoContext();
                    if (response.Exception == null)
                    {
                        status.Cost += response.Entity.CalculateCost();
                        var messageFromResponse = response.Entity.Result.Choices[0].Message.Content;
                        if (messageFromResponse != "false")
                            stringBuilder.AppendLine(messageFromResponse);
                    }
                }
            }
            var finalResponse = await openAi.Chat.Request(new ChatMessage
            {
                Content = request,
                Role = ChatRole.System
            })
                .AddMessage(new ChatMessage
                {
                    Content = stringBuilder.ToString(),
                    Role = ChatRole.User
                })
                .ExecuteAndCalculateCostAsync()
                .NoContext();
            status.Cost += finalResponse.CalculateCost();
            return new ChatMessage[1] { finalResponse.Result.Choices[0].Message };
        }
    }
}
