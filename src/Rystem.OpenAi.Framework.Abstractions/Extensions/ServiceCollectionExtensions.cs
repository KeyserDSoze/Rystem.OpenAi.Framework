using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            services
                .TryAddSingleton(OpenAiFrameworkConfiguration.Instance);
            services
                .TryAddTransient<IOpenAiAgentFactory, OpenAiAgentFactory>();
            services
                .TryAddTransient<IOpenAiAgent, OpenAiAgent>();
            services.
                TryAddTransient<IOpenAiTaskDefiner, ListTasksToDo>();
            services
                .TryAddTransient<IEmbeddingService, EmbeddingService>();
            return services;
        }
    }
}
