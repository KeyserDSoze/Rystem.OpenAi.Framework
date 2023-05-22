using System.Threading;
using System.Threading.Tasks;

namespace Rystem.OpenAi.Framework
{
    public interface IOpenAiAgent
    {
        void Set(IOpenAi openAi, string integrationName);
        ValueTask SolveTaskAsync(string taskDescription, CancellationToken cancellationToken = default);
    }
}
