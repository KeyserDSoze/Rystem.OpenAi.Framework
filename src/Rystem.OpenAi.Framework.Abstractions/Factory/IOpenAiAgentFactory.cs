namespace Rystem.OpenAi.Framework
{
    public interface IOpenAiAgentFactory
    {
        IOpenAiAgent CreateAgent(string integrationName);
    }
}
