using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rystem.OpenAi.Framework
{
    internal sealed class EmbeddingService : IEmbeddingService
    {
        private readonly IOpenAi _openAi;
        public EmbeddingService(IOpenAi openAi)
        {
            _openAi = openAi;
        }
        private const int MaxCharLength = 250 * 4;
        public async Task<List<EmbeddedInformation>> CalculateAsync(string text)
        {
            var chunks = new List<string>();
            var sentences = text.Replace('\n', ' ').Replace('\t', ' ').Replace('\r', ' ').Split('.');
            var currentChunk = string.Empty;
            // Create a variable to hold the current piece
            foreach (var sentence in sentences)
            {
                var trimmedSentence = sentence.Trim();
                if (string.IsNullOrWhiteSpace(trimmedSentence))
                    continue;

                var chunkLength = currentChunk.Length + trimmedSentence.Length + 1;
                var lowerBound = MaxCharLength - MaxCharLength * 0.5;
                var upperBound = MaxCharLength + MaxCharLength * 0.5;

                if (chunkLength >= lowerBound && chunkLength <= upperBound && currentChunk.Length != 0)
                {
                    currentChunk = currentChunk.Replace(".", string.Empty).Trim();
                    if (!string.IsNullOrWhiteSpace(currentChunk))
                        chunks.Add(currentChunk.ToString());
                    currentChunk = string.Empty;
                }
                else if (chunkLength > upperBound)
                {
                    currentChunk = currentChunk.Replace(".", string.Empty).Trim();
                    if (!string.IsNullOrWhiteSpace(currentChunk))
                        chunks.Add(currentChunk.ToString());
                    currentChunk = trimmedSentence;
                }
                else
                {
                    currentChunk = $"{currentChunk.Trim('.')}. {trimmedSentence.Trim('.')}.";
                }
                if (!string.IsNullOrWhiteSpace(currentChunk))
                    chunks.Add(currentChunk.ToString());
            }
            List<EmbeddedInformation> embeddingTexts = new();
            for (var i = 0; i < chunks.Count; i++)
            {
                var response = await _openAi.Embedding
                .Request(chunks[i])
                .WithModel(EmbeddingModelType.AdaTextEmbedding)
                .ExecuteAsync();
                embeddingTexts.Add(new EmbeddedInformation
                {
                    Value = chunks[i],
                    Array = response!.Data![0].Embedding!
                });
            }
            return embeddingTexts;
        }
    }
}
