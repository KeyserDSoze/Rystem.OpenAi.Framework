namespace Rystem.OpenAi.Framework
{
    public sealed class OpenAiResponse<TMessage>
        where TMessage : class
    {
        public TMessage Message { get; }
    }
}
