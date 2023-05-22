using System.Collections.Generic;

namespace Rystem.OpenAi.Framework
{
    public sealed class EmbeddedFile
    {
        public string Name { get; init; }
        public List<EmbeddedInformation> Pieces { get; init; }
    }
}
