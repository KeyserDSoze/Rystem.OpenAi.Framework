using System.Collections.Generic;

namespace Rystem.OpenAi.Framework
{
    public sealed class AgentStatus
    {
        public decimal Cost { get; set; }
        public List<OpenAiFrameworkResponse> Responses { get; } = new List<OpenAiFrameworkResponse>();
    }
}
