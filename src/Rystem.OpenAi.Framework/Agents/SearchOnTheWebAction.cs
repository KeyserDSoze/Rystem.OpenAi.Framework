using Rystem.OpenAi.Chat;

namespace Rystem.OpenAi.Framework
{
    internal sealed class SearchOnTheWebAction : IOpenAiAction
    {
        public string Id => "2";
        public string Description => "perform a search on the web";

        public ValueTask<ChatMessage[]> ExecuteAsync(IOpenAi openAi, string request)
        {
            throw new NotImplementedException();
        }
    }
}
