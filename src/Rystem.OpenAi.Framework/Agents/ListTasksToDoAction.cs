using Rystem.OpenAi.Chat;

namespace Rystem.OpenAi.Framework
{
    internal sealed class ListTasksToDoAction : IOpenAiAction
    {
        public string Id => "1";
        public string Description => "simplify the problem with a list of tasks";

        public ValueTask<ChatMessage[]> ExecuteAsync(IOpenAi openAi, string request)
        {
            throw new NotImplementedException();
        }
    }
}
