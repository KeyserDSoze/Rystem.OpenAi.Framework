using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rystem.OpenAi.Framework
{
    public interface IEmbeddingService
    {
        Task<List<EmbeddedInformation>> CalculateAsync(string text);
    }
}
