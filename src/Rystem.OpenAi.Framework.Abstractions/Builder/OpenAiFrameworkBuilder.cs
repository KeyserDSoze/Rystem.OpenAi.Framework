using Microsoft.Extensions.DependencyInjection.Extensions;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public sealed class OpenAiFrameworkBuilder
    {
        private readonly IServiceCollection _services;
        private readonly string _integrationName;

        internal OpenAiFrameworkBuilder(IServiceCollection services, string integrationName)
        {
            _services = services;
            _integrationName = integrationName;
        }
        public OpenAiFrameworkBuilder AddAction<TOpenAiAction>(int index, string actionDescription)
            where TOpenAiAction : class, IOpenAiAction
        {
            _services.AddTransient<IOpenAiAction, TOpenAiAction>();
            return this;
        }
        public OpenAiFrameworkBuilder AddAction<TOpenAiAction, TMessage>(int index, string actionDescription)
            where TOpenAiAction : class, IOpenAiAction<TMessage>
            where TMessage : class
        {
            _services.TryAddTransient<IOpenAiAction<TMessage>, TOpenAiAction>();
            return this;
        }
    }
}
