using Microsoft.Extensions.DependencyInjection.Extensions;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenAiFrameworkBuilderExtensions
    {
        public static OpenAiFrameworkBuilder AddDefaultActions(this OpenAiFrameworkBuilder builder,
            Action<WebSearchingSettings> webSearchSettings)
        {
            var options = new WebSearchingSettings();
            webSearchSettings.Invoke(options);
            builder.Services.TryAddSingleton(options);
            return builder
                .AddAction<ListTasksToDoAction>()
                .AddAction<WebSearchAction>();
        }
    }
}
