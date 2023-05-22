using System.Threading;
using System.Threading.Tasks;

namespace Rystem.OpenAi.Framework
{
    public interface IOpenAiTaskDefiner
    {
        ValueTask<AgentTasks> GetTasksAsync(string prompt, CancellationToken cancellationToken = default);
    }
}
