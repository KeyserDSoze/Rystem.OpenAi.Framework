using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rystem.OpenAi.Chat;

namespace Rystem.OpenAi.Framework
{
    internal sealed class OpenAiAgent : IOpenAiAgent
    {
        private IOpenAi? _openAi;
        private IOpenAi OpenAi => _openAi ??= _services.GetService<IOpenAi>()!;
        private readonly IOpenAiTaskDefiner _taskDefiner;
        private readonly IServiceProvider _services;
        private readonly IEnumerable<IOpenAiAction> _actions;
        private readonly OpenAiFrameworkConfiguration _configuration;
        public OpenAiAgent(IServiceProvider services, IOpenAiTaskDefiner taskDefiner, IEnumerable<IOpenAiAction> actions, OpenAiFrameworkConfiguration configuration)
        {
            _services = services;
            _taskDefiner = taskDefiner;
            _actions = actions;
            _configuration = configuration;
        }
        public void Set(IOpenAi openAi, string integrationName)
        {
            _taskDefiner.Set(openAi);
            _openAi = openAi;
            _integrationName = integrationName;
        }
        private string _integrationName = string.Empty;
        private readonly AgentStatus _status = new();
        private readonly Queue<ChatMessage[]> _queue = new();
        public async ValueTask SolveTaskAsync(string taskDescription, CancellationToken cancellationToken = default)
        {
            var taskWrapper = await _taskDefiner.GetTasksAsync(taskDescription, cancellationToken);
            foreach (var task in taskWrapper.Tasks)
            {
                
            }
        }
        private async ValueTask ExecuteRequestAsync(CancellationToken cancellationToken, string taskDescription, params ChatMessage[] messages)
        {
            var request = _openAi.Chat.Request(new ChatMessage
            {
                Role = ChatRole.System,
                Content = _configuration.GetSystemMessage(_integrationName)
            });
            foreach (var message in messages)
                request.AddMessage(message);
            var response = await request
                .WithTemperature(0)
                .ExecuteAndCalculateCostAsync(cancellationToken);
            var messageResponse = response!.Result!.Choices![0].Message!.Content!.FromJson<AgentAction>();
            _status.Cost += response.CalculateCost();
            _status.Tasks.Add(messageResponse);
            foreach (var action in messageResponse.Thought.Actions)
            {
                var actionToDo = _actions.FirstOrDefault(x => x.Id == action.CommandId);
                if (actionToDo != null)
                {
                    _queue.Enqueue(await actionToDo.ExecuteAsync(_openAi, messageResponse.Thought, taskDescription, action.ActionToDo, _status, cancellationToken).NoContext());
                }
            }
        }
    }
}
