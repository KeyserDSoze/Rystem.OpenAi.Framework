using System.Collections.Generic;
using System.Threading.Tasks;
using Rystem.OpenAi.Chat;

namespace Rystem.OpenAi.Framework
{
    public interface IOpenAiAction
    {
        string Id { get; }
        string Description { get; }
        ValueTask<ChatMessage[]> ExecuteAsync(IOpenAi openAi, string request);
    }
    public interface IOpenAiAction<in TMessage> : IOpenAiAction
        where TMessage : class
    {
        ValueTask ExecuteAsync(TMessage message);
    }
}
