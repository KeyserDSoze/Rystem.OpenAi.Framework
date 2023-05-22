using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Rystem.OpenAi.Framework
{
    internal sealed class ListTasksToDo : IOpenAiTaskDefiner
    {
        private readonly IOpenAi _openAi;
        public ListTasksToDo(IOpenAi openAi)
        {
            _openAi = openAi;
        }
        private static readonly string s_systemPrompt = $"You are an assistant that helps user to reach his goal. You must provide a set of tasks that can help to achieve the goal. You must only respond in JSON format as described delimited by triple backticks '''{AgentTasks.TasksAsJson}'''";
        public async ValueTask<AgentTasks> GetTasksAsync(string prompt, CancellationToken cancellationToken = default)
        {
            var response = await _openAi.Chat
                .RequestWithSystemMessage(s_systemPrompt)
                .AddUserMessage(prompt)
                .ExecuteAndCalculateCostAsync(cancellationToken);
            return response.Result.Choices[0].Message.Content.FromJson<AgentTasks>();
        }
    }
}
