using System.Text.Json;
using Newtonsoft.Json;

namespace Rystem.OpenAi.Framework.Tests
{
    public class PromptTest
    {
        private readonly IOpenAiFactory _openAiFactory;
        public PromptTest(IOpenAiFactory openAiFactory)
        {
            _openAiFactory = openAiFactory;
        }
        [Theory]
        [InlineData("Azure", "Prepare a travel to Italy in Rome from Berlin for 15-25 august 2023.")]
        [InlineData("Azure", "Create an article for a blog about the movie Avatar 2 with a positive mood.")]
        [InlineData("Azure", "Create a web site blog about movies review, at least 10 movies until 2020 need to be reviewed. Generate an HTML ouput with boostrap css.")]
        [InlineData("Azure", "Vorrei avere un preventivo per la ristrutturazione di un tetto di castagno di 100mq")]
        public async Task TestAsync(string integrationName, string taskToDo)
        {
            var openAi = _openAiFactory.Create(integrationName);
            var preScheduled = openAi.Chat.RequestWithSystemMessage("You are an assistant that helps user to reach his goal." +
                "You must provide a set of tasks that can help to achieve the goal." +
                $"You must only respond in JSON format as described delimited by triple backticks '''{s_tasks}'''");
            var preResponse = await preScheduled
                                .AddUserMessage(taskToDo)
                                .ExecuteAsync();
            var preMessage = preResponse.Choices[0].Message.Content;
            var preContent = preMessage.FromJson<List<AgentTask>>();
            foreach (var task in preContent)
            {
                Assert.NotNull(task.Description);

                var chat = openAi.Chat.RequestWithSystemMessage(
                    "You are an assistant that helps user to reach several goals." +
                    " You can improve your response with some commands, " +
                    "Commands: " +
                    $"[" +
                    "{\"commandId\":\"1\",\"description\":\"perform a search on the web\"}]." +
                    $"You should only respond in JSON format as described below \nResponse Format: \n{s_defaultResponse}");
                var response = await chat.AddUserMessage(task.Description)
                    .ExecuteAsync();
                var responseAsJson = response.Choices[0].Message.Content;
                Assert.NotEmpty(responseAsJson);
                var possibleResponse = responseAsJson.FromJson<Framework.AgentAction>();
                Assert.NotNull(possibleResponse);
                Assert.NotNull(possibleResponse.Thought.Title);
                Assert.NotNull(possibleResponse.Thought.Subject);
                foreach (var action in possibleResponse.Thought.Actions)
                {
                    Assert.NotNull(action.CommandId);
                    Assert.NotNull(action.ActionToDo);
                }
            }
        }
        private sealed class AgentTask
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
        }
        private static readonly string s_tasks = new List<AgentTask>()
        {
            new AgentTask
            {
                Title="Insert here a title for the task, max 4 words",
                Description = "Insert here a detailed description for the task"
            }
        }.ToJson();
        private static readonly string s_defaultResponse = new Framework.AgentAction()
        {
            Thought = new OpenAiThought
            {
                Text = "thought",
                Reason = "reasoning",
                Criticism = "constructive self-criticism",
                Speak = "thoughts summary to say to user",
                Title = "create a title for the request",
                Subject = "insert here the subject of the request, max 4 words",
                Actions = new List<PlannedAction>
                 {
                     new PlannedAction
                     {
                         ActionToDo = "Action to do from a list of actions with a detailed description",
                         CommandId = "The command id to use to solve the action"
                     }
                 }
            }
        }.ToJson();
    }
}
