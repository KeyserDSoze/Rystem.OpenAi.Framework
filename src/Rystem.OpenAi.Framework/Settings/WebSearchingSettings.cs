namespace Rystem.OpenAi.Framework
{
    public sealed class WebSearchingSettings
    {
        public string BingWebSearchSubscriptionKey { get; set; }
        /// <summary>
        /// With false the web search will use the HttpClient that is faster but javascript will not run.
        /// </summary>
        public bool WithSelenium { get; set; } = true;
    }
}
