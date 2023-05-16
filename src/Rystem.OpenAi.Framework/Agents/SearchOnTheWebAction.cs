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
        public async ValueTask<ChatMessage[]> ExecuteAsync(IOpenAi openAi, string request, CancellationToken cancellationToken = default)
        {
            await foreach (var x in _webSearchService.SearchAsync(request, cancellationToken))
            {
                string olaf = x.ToString();
            }
            throw new NotImplementedException();
        }
    }
}
