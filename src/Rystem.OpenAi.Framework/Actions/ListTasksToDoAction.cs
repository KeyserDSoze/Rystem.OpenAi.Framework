using System.Text.Json;
using Rystem.OpenAi.Chat;

namespace Rystem.OpenAi.Framework
{
    internal sealed class ListTasksToDoAction : IOpenAiAction
    {
        public string Id => "1";
        public string Description => $"simplify the problem with a list of tasks in this json format '''{InnerTaskAsJson}'''";
        private static readonly string InnerTaskAsJson = new List<InnerTask>
        {
            new InnerTask
            {
                Id = 1,
                Description = "First task",
            },
            new InnerTask
            {
                Id = 2,
                Description = "Second task",
            }
        }.ToJson();
        public ValueTask<ChatMessage[]> ExecuteAsync(IOpenAi openAi, OpenAiThought thought, string startingRequest, string request, AgentStatus status, CancellationToken cancellationToken = default)
        {
            var array = request.FromJson<List<InnerTask>>();
            return ValueTask.FromResult(array.Select(x => new ChatMessage
            {
                Content = x.Description,
                Role = ChatRole.System
            }).ToArray());
        }
        private sealed class InnerTask
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }
    }
}
