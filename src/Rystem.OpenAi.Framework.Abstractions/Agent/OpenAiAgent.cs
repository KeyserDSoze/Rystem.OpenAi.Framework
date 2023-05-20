using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rystem.OpenAi.Chat;

namespace Rystem.OpenAi.Framework
{
    public sealed class AgentStatus
    {
        public decimal Cost { get; set; }
        public List<OpenAiFrameworkResponse> Responses { get; } = new List<OpenAiFrameworkResponse>();
    }
    internal sealed class OpenAiAgent : IOpenAiAgent
    {
        private readonly IOpenAi _openAi;
        private readonly IEnumerable<IOpenAiAction> _actions;
        private readonly OpenAiFrameworkConfiguration _configuration;
        public OpenAiAgent(IOpenAi openAi, IEnumerable<IOpenAiAction> actions, OpenAiFrameworkConfiguration configuration)
        {
            _openAi = openAi;
            _actions = actions;
            _configuration = configuration;
        }
        public string IntegrationName { get; init; } = string.Empty;
        private readonly AgentStatus _status = new();
        private readonly Queue<ChatMessage[]> _queue = new();
        public async ValueTask SolveTaskAsync(string taskDescription, CancellationToken cancellationToken = default)
        {
            await ExecuteRequestAsync(cancellationToken, new ChatMessage
            {
                Content = taskDescription,
                Role = ChatRole.User
            }).NoContext();
            while (_queue.Count > 0)
            {
                var messages = _queue.Dequeue();
                await ExecuteRequestAsync(cancellationToken, messages);
            }
        }
        private async ValueTask ExecuteRequestAsync(CancellationToken cancellationToken, params ChatMessage[] messages)
        {
            var request = _openAi.Chat.Request(new ChatMessage
            {
                Role = ChatRole.System,
                Content = _configuration.GetSystemMessage(IntegrationName)
            });
            foreach (var message in messages)
                request.AddMessage(message);
            var response = await request
                .WithTemperature(0)
                .ExecuteAndCalculateCostAsync(cancellationToken);
            var messageResponse = response!.Result!.Choices![0].Message!.Content!.FromJson<OpenAiFrameworkResponse>();
            _status.Cost += response.CalculateCost();
            _status.Responses.Add(messageResponse);
            foreach (var action in messageResponse.Thought.Actions)
            {
                var actionToDo = _actions.FirstOrDefault(x => x.Id == action.CommandId);
                if (actionToDo != null)
                {
                    _queue.Enqueue(await actionToDo.ExecuteAsync(_openAi, action.ActionToDo, _status, cancellationToken).NoContext());
                }
            }
        }
    }
}
