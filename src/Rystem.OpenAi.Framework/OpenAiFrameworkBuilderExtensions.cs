using Microsoft.Extensions.DependencyInjection.Extensions;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenAiFrameworkBuilderExtensions
    {
        public static OpenAiFrameworkBuilder AddDefaultActions(this OpenAiFrameworkBuilder builder,
            Action<OpenAiFrameworkDefaultSettings> settings)
        {
            var options = new OpenAiFrameworkDefaultSettings();
            settings.Invoke(options);
            builder.Services.TryAddSingleton(options);
            return builder
                .AddAction<ListTasksToDoAction>()
                .AddAction<SearchOnTheWebAction>();
        }
    }
}
