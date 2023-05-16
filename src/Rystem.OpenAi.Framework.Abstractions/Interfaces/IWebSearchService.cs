using System.Collections.Generic;
using System.Threading;

namespace Rystem.OpenAi.Framework
{
    public interface IWebSearchService
    {
        IAsyncEnumerable<WebSearchPage> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}
