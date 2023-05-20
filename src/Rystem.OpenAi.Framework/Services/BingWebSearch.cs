using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Bing.WebSearch;

namespace Rystem.OpenAi.Framework
{
    internal sealed class BingWebSearch : IWebSearchService
    {
        private readonly WebSearchClient _client;
        private readonly HttpClient _httpClient;
        internal const string HttpClientName = "webSearchClientFromBing";
        //private const string BodyStart = "<body";
        //private const string BodyEnd = "</body>";
        //private static readonly Regex ScriptRegex = new(@"<script[\s\S]*?>[\s\S]*?<\/script>");
        //private static readonly Regex BodyRegex = new(@"<body[\s\S]*?>[\s\S]*?<\/body>");
        //private static readonly Regex StyleRegex = new(@"<style[\s\S]*?>[\s\S]*?<\/style>");
        public BingWebSearch(OpenAiFrameworkDefaultSettings openAiFrameworkDefaultSettings,
            IHttpClientFactory httpClientFactory)
        {
            _client = new WebSearchClient(
                new ApiKeyServiceClientCredentials(openAiFrameworkDefaultSettings.BingWebSearchSubscriptionKey));
            _httpClient = httpClientFactory.CreateClient(HttpClientName);
        }
        public async IAsyncEnumerable<WebSearchPage> SearchAsync(string query,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var results = (await _client.Web.SearchAsync(query));
            foreach (var result in results.WebPages.Value.Take(4))
            {
                var value = await _httpClient.GetAsync(result.Url).NoContext();
                using var stream = await value.Content.ReadAsStreamAsync().NoContext();
                using var reader = new StreamReader(stream);
                var html = await reader.ReadToEndAsync().NoContext();
                //var body = BodyRegex.Match(html).Value;
                //var matches = ScriptRegex.Matches(body);
                //foreach (var scriptMatch in matches)
                //{
                //    body = body.Replace(scriptMatch.ToString(), string.Empty);
                //}
                //foreach (var styleMatch in StyleRegex.Matches(body))
                //{
                //    body = body.Replace(styleMatch.ToString(), string.Empty);
                //}
                yield return new WebSearchPage
                {
                    Content = html,
                    Description = result.Snippet,
                    Uri = result.Url
                };
            }
        }
    }
}
