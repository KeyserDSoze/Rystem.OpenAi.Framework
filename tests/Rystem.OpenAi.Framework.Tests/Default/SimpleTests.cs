namespace Rystem.OpenAi.Framework.Tests
{
    public class SimpleTests
    {
        private readonly IOpenAiAgentFactory _openAiAgentFactory;
        public SimpleTests(IOpenAiAgentFactory openAiAgentFactory)
        {
            _openAiAgentFactory = openAiAgentFactory;
        }
        [Theory]
        [InlineData("Azure", "Prepare a travel to Italy in Rome for 15-25 august 2023.")]
        [InlineData("Azure", "Create an article for a blog about the movie Avatar 2 with a positive mood.")]
        public async Task TestAsync(string integrationName, string taskToDo)
        {
            var agent = _openAiAgentFactory.CreateAgent(integrationName);
            await agent.SolveTaskAsync(taskToDo);
        }
    }
}
