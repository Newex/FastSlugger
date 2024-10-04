using System.Globalization;
using AnyAscii;
using CommunityToolkit.HighPerformance.Buffers;

namespace FastSlugger;

/// <summary>
/// The slug creator
/// </summary>
public static class Slug
{
    /// <summary>
    /// Create a slug from a given text input
    /// </summary>
    /// <param name="input">The text input</param>
    /// <param name="includeNumbers">If numbers should be included, or skipped.</param>
    /// <param name="connector">The connector character</param>
    /// <param name="separator">The separator character</param>
    /// <param name="punctuation">The punctuation character</param>
    /// <param name="max">The maximum string length. The number of characters in the resulting string</param>
    /// <returns>A slug</returns>
    public static string Create(string input, bool includeNumbers = true, char connector = '_', char separator = '-', char? punctuation = null, int? max = null)
    {
        var length = Math.Min(input.Length, max.GetValueOrDefault());
        var chunk = !max.HasValue ? input : input[0..length];
        var transliterate = chunk.Transliterate();
        return ToLowercaseAndHyphenSeparator(transliterate, includeNumbers, connector, separator, punctuation, max);
    }

    private static string ToLowercaseAndHyphenSeparator(string text, bool includeNumbers, char connector, char separator, char? punctuation, int? max)
    {
        var limit = max ?? text.Length;
        Span<char> span = limit < 1000
            ? stackalloc char[limit]
            : new char[limit];

        var j = 0;
        for (var i = 0; j < limit && i < text.Length; i++)
        {
            var c = text[i];
            var info = CharUnicodeInfo.GetUnicodeCategory(c);
            switch (info)
            {
                case UnicodeCategory.OtherPunctuation:
                    if (!punctuation.HasValue)
                    {
                        continue;
                    }
                    else
                    {
                        if (j - 1 >= 0)
                        {
                            var prevChar = span[j - 1];
                            if (prevChar == punctuation.Value)
                            {
                                continue;
                            }
                        }
                        span[j++] = punctuation.Value;
                        break;
                    }
                case UnicodeCategory.UppercaseLetter:
                    span[j++] = char.ToLowerInvariant(c);
                    break;
                case UnicodeCategory.LowercaseLetter:
                    span[j++] = c;
                    break;
                case UnicodeCategory.DecimalDigitNumber:
                    if (includeNumbers)
                    {
                        span[j++] = c;
                    }
                    else
                    {
                        j++;
                    }
                    break;
                case UnicodeCategory.ConnectorPunctuation:
                    if (j - 1 >= 0)
                    {
                        // Truncate multiple connectors into a single character
                        var prevChar = span[j - 1];
                        if (prevChar == connector)
                        {
                            continue;
                        }
                    }
                    span[j++] = connector;
                    break;
                default:
                    if (j - 1 >= 0)
                    {
                        // Truncate separators
                        var prevChar = span[j - 1];
                        if (prevChar == separator)
                        {
                            continue;
                        }
                    }
                    span[j++] = separator;
                    break;
            }
        }

        return StringPool.Shared.GetOrAdd(span[0..j]);
    }
}
