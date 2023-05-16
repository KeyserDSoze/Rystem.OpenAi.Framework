using System.Runtime.CompilerServices;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;

namespace Rystem.OpenAi.Framework
{
    internal sealed class BingWebSearch : IWebSearchService
    {
        private readonly WebSearchClient _client;
        public BingWebSearch(OpenAiFrameworkDefaultSettings openAiFrameworkDefaultSettings)
        {
            _client = new WebSearchClient(new ApiKeyServiceClientCredentials(openAiFrameworkDefaultSettings.BingWebSearchSubscriptionKey));
        }
        public async IAsyncEnumerable<WebSearchPage> SearchAsync(string query, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var results = (await _client.Web.SearchAsync(query));
            yield return new WebSearchPage { };
        }
    }
}
