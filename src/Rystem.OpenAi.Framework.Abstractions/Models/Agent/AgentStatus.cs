using System;
using System.Collections.Generic;

namespace Rystem.OpenAi.Framework
{
    public sealed class AgentStatus
    {
        public string Id => Information.Id;
        public decimal Cost { get; set; }
        public List<AgentAction> Tasks { get; } = new List<AgentAction>();
        public EmbeddedPackage Information { get; } = new()
        {
            Id = Guid.NewGuid().ToString(),
            Pieces = new()
        };
    }
}
