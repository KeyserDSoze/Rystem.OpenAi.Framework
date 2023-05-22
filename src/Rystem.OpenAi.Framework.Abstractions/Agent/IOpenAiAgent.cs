using System.Threading;
using System.Threading.Tasks;

namespace Rystem.OpenAi.Framework
{
    public interface IOpenAiAgent
    {
        ValueTask SolveTaskAsync(string taskDescription, CancellationToken cancellationToken = default);
    }
}
