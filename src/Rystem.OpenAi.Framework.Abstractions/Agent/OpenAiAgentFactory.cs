using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Rystem.OpenAi.Framework
{
    internal sealed class OpenAiAgentFactory : IOpenAiAgentFactory
    {
        private readonly IOpenAiFactory _openAiFactory;
        private readonly IEnumerable<IOpenAiAction> _actions;
        private readonly OpenAiFrameworkConfiguration _configuration;

        public OpenAiAgentFactory(IOpenAiFactory openAiFactory, IEnumerable<IOpenAiAction> actions, OpenAiFrameworkConfiguration configuration)
        {
            _openAiFactory = openAiFactory;
            _actions = actions;
            _configuration = configuration;
        }

        public IOpenAiAgent CreateAgent(string integrationName)
            => new OpenAiAgent(_openAiFactory.Create(integrationName), _actions, _configuration)
            {
                IntegrationName = integrationName
            };
    }
}
