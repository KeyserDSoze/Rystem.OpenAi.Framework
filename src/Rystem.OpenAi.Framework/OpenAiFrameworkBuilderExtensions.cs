using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenAiFrameworkBuilderExtensions
    {
        public static OpenAiFrameworkBuilder AddDefaultActions(this OpenAiFrameworkBuilder builder)
        {
            return builder
                .AddAction<ListTasksToDoAction>(1, "simplify the problem with a list of tasks")
                .AddAction<SearchOnTheWebAction>(2, "perform a search on the web");
        }
    }
}
