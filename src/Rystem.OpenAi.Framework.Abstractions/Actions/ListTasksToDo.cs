using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Rystem.OpenAi.Framework
{
    internal sealed class ListTasksToDo : IOpenAiTaskDefiner
    {
        private IOpenAi? _openAi;
        private IOpenAi OpenAi => _openAi ??= _services.GetService<IOpenAi>()!;
        public ListTasksToDo(IServiceProvider services)
        {
            _services = services;
        }
        private static readonly string s_systemPrompt = $"You are an assistant that helps user to reach his goal. You must provide a set of tasks that can help to achieve the goal and show it in order of priority. You must only respond in JSON format as described delimited by triple backticks '''{AgentTasks.TasksAsJson}'''";
        private readonly IServiceProvider _services;

        public async ValueTask<AgentTasks> GetTasksAsync(string prompt, CancellationToken cancellationToken = default)
        {
            var response = await OpenAi.Chat
                .RequestWithSystemMessage(s_systemPrompt)
                .AddUserMessage(prompt)
                .ExecuteAndCalculateCostAsync(cancellationToken);
            return response.Result.Choices[0].Message.Content.FromJson<AgentTasks>();
        }

        public void Set(IOpenAi openAi)
        {
            _openAi = openAi;
        }
    }
}
