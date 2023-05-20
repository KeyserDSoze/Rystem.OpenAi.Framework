using System.Text.Json.Serialization;

namespace Rystem.OpenAi.Framework
{
    public sealed class OpenAiFrameworkResponse
    {
        [JsonPropertyName("thought")]
        public OpenAiThought Thought { get; set; }
    }
}
