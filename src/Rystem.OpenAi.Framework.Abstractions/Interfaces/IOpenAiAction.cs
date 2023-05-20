using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rystem.OpenAi.Chat;

namespace Rystem.OpenAi.Framework
{
    public interface IOpenAiAction
    {
        string Id { get; }
        string Description { get; }
        ValueTask<ChatMessage[]> ExecuteAsync(IOpenAi openAi, string request, AgentStatus status, CancellationToken cancellationToken = default);
    }
}
