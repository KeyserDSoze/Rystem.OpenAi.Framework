using System.Text.Json.Serialization;

namespace Rystem.OpenAi.Framework
{
    public sealed class AgentTask
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
