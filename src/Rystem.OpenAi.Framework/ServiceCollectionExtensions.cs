using Microsoft.Extensions.DependencyInjection.Extensions;
using Rystem.OpenAi;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenAiFrameworkWithDefaultActions(this IServiceCollection services,
            Action<OpenAiSettings> settings,
            Action<OpenAiFrameworkBuilder> builder,
            Action<WebSearchingSettings> webSearchSettings,
            string? integrationName = default)
        {
            services
                .TryAddTransient<IWebSearchService, BingWebSearch>();
            services
                .AddHttpClient(BingWebSearch.HttpClientName, configuration =>
                {
                    configuration.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");
                });
            return services.AddOpenAiFramework(settings, build =>
            {
                build
                    .AddDefaultActions(webSearchSettings);
                builder
                    .Invoke(build);
            }, integrationName);
        }
    }
}
