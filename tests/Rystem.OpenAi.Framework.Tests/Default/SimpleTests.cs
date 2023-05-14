﻿namespace Rystem.OpenAi.Framework.Tests
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
        public async Task TestAsync(string integrationName, string taskToDo)
        {
            var agent = _openAiAgentFactory.CreateAgent(integrationName);
            await agent.SolveTaskAsync(taskToDo);
        }
    }
}