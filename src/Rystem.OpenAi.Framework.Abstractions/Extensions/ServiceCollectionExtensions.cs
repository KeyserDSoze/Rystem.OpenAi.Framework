using System;
using Rystem.OpenAi;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenAiFramework(this IServiceCollection services,
            Action<OpenAiSettings> settings,
            Action<OpenAiFrameworkBuilder> builder,
            string? integrationName = default)
        {
            var build = new OpenAiFrameworkBuilder(services, integrationName ?? string.Empty);
            builder.Invoke(build);
            services
                .AddOpenAi(settings, integrationName);
            services.AddTransient<IOpenAiAgentFactory, OpenAiAgentFactory>();
            services.AddTransient<IOpenAiAgent, OpenAiAgent>();
            return services;
        }
    }
}
