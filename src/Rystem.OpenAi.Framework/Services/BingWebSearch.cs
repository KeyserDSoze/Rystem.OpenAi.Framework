using System.Runtime.CompilerServices;
using HtmlAgilityPack;
using Microsoft.Bing.WebSearch;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Rystem.OpenAi.Framework
{
    internal sealed class BingWebSearch : IWebSearchService
    {
        private readonly WebSearchClient _client;
        private readonly HttpClient _httpClient;
        private readonly WebSearchingSettings _webSearchSettings;
        internal const string HttpClientName = "webSearchClientFromBing";

        public BingWebSearch(WebSearchingSettings webSearchSettings,
            IHttpClientFactory httpClientFactory)
        {
            _client = new WebSearchClient(
                new ApiKeyServiceClientCredentials(webSearchSettings.BingWebSearchSubscriptionKey));
            _httpClient = httpClientFactory.CreateClient(HttpClientName);
            _webSearchSettings = webSearchSettings;
        }
        public async IAsyncEnumerable<WebSearchPage> SearchAsync(string query,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var results = (await _client.Web.SearchAsync(query, count: 100));
            foreach (var result in results.WebPages.Value)
            {
                var text = string.Empty;
                if (!_webSearchSettings.WithSelenium)
                {
                    var value = await _httpClient.GetAsync(result.Url).NoContext();
                    using var stream = await value.Content.ReadAsStreamAsync().NoContext();
                    using var reader = new StreamReader(stream);
                    var html = await reader.ReadToEndAsync().NoContext();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    text = doc.DocumentNode.SelectSingleNode("//body").InnerText
                        .Replace('\n'.ToString(), string.Empty)
                        .Replace('\t'.ToString(), string.Empty);
                }
                else
                {
                    var options = new ChromeOptions();
                    options.SetLoggingPreference("performance", LogLevel.All);
                    options.AddUserProfilePreference("intl.accept_languages", "en-US");
                    options.AddUserProfilePreference("disable-popup-blocking", "true");
                    options.AddArgument("--headless");
                    options.AddArgument("--disable-gpu");
                    options.AddArgument("--no-sandbox");
                    options.LeaveBrowserRunning = false;
                    IWebDriver driver = new ChromeDriver(options);
                    try
                    {
                        driver.Navigate().GoToUrl(result.Url);
                    }
                    catch
                    {
                        continue;
                    }
                    await Task.Delay(1000);
                    text = driver.FindElement(By.TagName("body")).Text;
                }
                yield return new WebSearchPage
                {
                    Content = text,
                    Description = result.Snippet,
                    Uri = result.Url
                };
            }
        }
    }
}
