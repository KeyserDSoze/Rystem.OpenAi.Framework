using System.Collections.Generic;

namespace Rystem.OpenAi.Framework
{
    public sealed class EmbeddedPackage
    {
        public string Id { get; init; }
        public List<EmbeddedFile> Pieces { get; init; }
    }
}
