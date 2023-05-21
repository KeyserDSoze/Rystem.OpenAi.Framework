using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rystem.OpenAi.Framework
{
    public sealed class OpenAiThought
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = null!;
        [JsonPropertyName("reasoning")]
        public string Reason { get; set; }
        [JsonPropertyName("actions")]
        public List<PlannedAction> Actions { get; set; }
        [JsonPropertyName("criticism")]
        public string Criticism { get; set; }
        [JsonPropertyName("speak")]
        public string Speak { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
