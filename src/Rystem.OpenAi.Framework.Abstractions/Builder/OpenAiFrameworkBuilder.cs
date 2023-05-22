using System;
using System.Collections.Generic;
using RepositoryFramework;
using Rystem.OpenAi.Framework;

namespace Microsoft.Extensions.DependencyInjection
{
    public sealed class OpenAiFrameworkBuilder
    {
        public IServiceCollection Services { get; }
        private readonly string _integrationName;
        internal OpenAiFrameworkBuilder(IServiceCollection services, string integrationName)
        {
            Services = services;
            _integrationName = integrationName;
            if (!OpenAiFrameworkConfiguration.Instance.MappedActions.ContainsKey(_integrationName))
                OpenAiFrameworkConfiguration.Instance.MappedActions.Add(_integrationName, new List<Type>());
        }
        public OpenAiFrameworkBuilder AddAction<TOpenAiAction>()
            where TOpenAiAction : class, IOpenAiAction
        {
            Services
                .AddTransient<IOpenAiAction, TOpenAiAction>();
            OpenAiFrameworkConfiguration.Instance.MappedActions[_integrationName].Add(typeof(TOpenAiAction));
            return this;
        }
        public OpenAiFrameworkBuilder AddEmbeddingStorage(Action<RepositorySettings<EmbeddedPackage, string>> settings)
        {
            Services
                .AddRepository(settings);
            return this;
        }
    }
}
