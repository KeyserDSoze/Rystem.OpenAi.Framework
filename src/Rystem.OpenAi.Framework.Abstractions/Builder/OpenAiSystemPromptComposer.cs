using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class OpenAiSystemPromptComposer
    {
        public static Dictionary<string, StringBuilder> Composer { get; } = new Dictionary<string, StringBuilder>();
    }
}
