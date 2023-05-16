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
            Action<OpenAiFrameworkDefaultSettings> defaultSettings,
            string? integrationName = default)
        {
            services
                .TryAddTransient<IWebSearchService, BingWebSearch>();
            return services.AddOpenAiFramework(settings, build =>
            {
                build
                    .AddDefaultActions(defaultSettings);
                builder
                    .Invoke(build);
            }, integrationName);
        }
    }
}
