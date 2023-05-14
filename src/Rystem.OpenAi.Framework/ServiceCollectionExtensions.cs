using Rystem.OpenAi;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenAiFrameworkWithDefaultActions(this IServiceCollection services,
            Action<OpenAiSettings> settings,
            Action<OpenAiFrameworkBuilder> builder,
            string? integrationName = default)
        {
            return services.AddOpenAiFramework(settings, build =>
            {
                build
                    .AddDefaultActions();
                builder
                    .Invoke(build);
            }, integrationName);
        }
    }
}
