using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rystem.OpenAi.Framework
{
    public sealed class OpenAiFrameworkResponse
    {
        [JsonPropertyName("thought")]
        public OpenAiThought Thought { get; set; }
    }
}
