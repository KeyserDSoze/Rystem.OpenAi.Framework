using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class OpenAiSystemPromptComposer
    {
        public static StringBuilder Composer { get; } = new StringBuilder();
        public static string FinalComposition { get; set; } = null!;
    }
}
