using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rystem.OpenAi.Framework
{
    public sealed class AgentAction
    {
        [JsonPropertyName("thought")]
        public OpenAiThought Thought { get; set; }
        [JsonPropertyName("memory")]
        public List<EmbeddedFile> Files { get; set; }
    }
}
