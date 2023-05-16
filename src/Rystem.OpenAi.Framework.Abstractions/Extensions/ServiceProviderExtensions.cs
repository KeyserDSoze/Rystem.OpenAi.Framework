using System;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider MapOpenAiFrameworkActions(this IServiceProvider serviceProvider)
        {
            var actions = serviceProvider.GetServices<IOpenAiAction>();
            var configuration = serviceProvider.GetService<OpenAiFrameworkConfiguration>()!;
            configuration.Compose(actions);
            return serviceProvider;
        }

    }
}
