using System.Collections;
using System.Collections.Generic;

namespace Rystem.OpenAi.Framework
{
    internal sealed class OpenAiAgentFactory : IOpenAiAgentFactory
    {
        private readonly IOpenAiFactory _openAiFactory;
        private readonly IEnumerable<IOpenAiAction> _actions;

        public OpenAiAgentFactory(IOpenAiFactory openAiFactory, IEnumerable<IOpenAiAction> actions)
        {
            _openAiFactory = openAiFactory;
            _actions = actions;
        }

        public IOpenAiAgent CreateAgent(string integrationName)
            => new OpenAiAgent(_openAiFactory.Create(integrationName), _actions)
            {
                IntegrationName = integrationName
            };
    }
}
