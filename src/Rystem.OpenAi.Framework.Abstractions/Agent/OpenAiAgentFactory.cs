namespace Rystem.OpenAi.Framework
{
    internal sealed class OpenAiAgentFactory : IOpenAiAgentFactory
    {
        private readonly IOpenAiFactory _openAiFactory;
        private readonly IOpenAiAgent _agent;

        public OpenAiAgentFactory(IOpenAiFactory openAiFactory, IOpenAiAgent agent)
        {
            _openAiFactory = openAiFactory;
            _agent = agent;
        }

        public IOpenAiAgent CreateAgent(string integrationName)
        {
            _agent.Set(_openAiFactory.Create(integrationName), integrationName);
            return _agent;
        }
    }
}
