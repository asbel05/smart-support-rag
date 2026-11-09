namespace SmartSupportRag.Api.Services;

public class TextChunker
{
    public IReadOnlyList<string> ChunkText(string text, int maxChunkSize = 600, int overlap = 100)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        var normalized = text.Replace("\r\n", "\n").Trim();
        if (normalized.Length <= maxChunkSize)
            return [normalized];

        var chunks = new List<string>();
        var start = 0;

        while (start < normalized.Length)
        {
            var length = Math.Min(maxChunkSize, normalized.Length - start);
            var end = start + length;

            if (end < normalized.Length)
            {
                var searchArea = normalized.Substring(start, length);
                var breakPos = FindBestBreakPosition(searchArea);

                if (breakPos > maxChunkSize / 3)
                {
                    end = start + breakPos;
                }
            }

            var chunk = normalized[start..end].Trim();
            if (!string.IsNullOrEmpty(chunk))
            {
                chunks.Add(chunk);
            }

            if (end >= normalized.Length)
                break;

            start = Math.Max(start + 1, end - overlap);

            while (start < normalized.Length && char.IsWhiteSpace(normalized[start]))
            {
                start++;
            }
        }

        return chunks;
    }

    private static int FindBestBreakPosition(string segment)
    {
        var paragraphBreak = segment.LastIndexOf("\n\n", StringComparison.Ordinal);
        if (paragraphBreak != -1)
            return paragraphBreak + 2;

        var lineBreak = segment.LastIndexOf('\n');
        if (lineBreak != -1)
            return lineBreak + 1;

        var sentenceEnd = segment.LastIndexOf(". ", StringComparison.Ordinal);
        if (sentenceEnd != -1)
            return sentenceEnd + 2;

        var space = segment.LastIndexOf(' ');
        if (space != -1)
            return space + 1;

        return segment.Length;
    }
}

