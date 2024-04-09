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
    /// <param name="connector">The connector character</param>
    /// <param name="separator">The separator character</param>
    /// <returns>A slug</returns>
    public static string Create(string input, char connector = '_', char separator = '-')
    {
        var transliterate = input.Transliterate();
        return ToLowercaseAndHyphenSeparator(transliterate, connector, separator);
    }

    private static string ToLowercaseAndHyphenSeparator(string text, char connector, char separator)
    {
        Span<char> span = text.Length < 1000
            ? stackalloc char[text.Length]
            : new char[text.Length];

        var j = 0;
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            var info = CharUnicodeInfo.GetUnicodeCategory(c);
            switch (info)
            {
                case UnicodeCategory.OtherPunctuation:
                    continue;
                case UnicodeCategory.UppercaseLetter:
                    span[j++] = char.ToLowerInvariant(c);
                    break;
                case UnicodeCategory.LowercaseLetter:
                    span[j++] = c;
                    break;
                case UnicodeCategory.ConnectorPunctuation:
                    span[j++] = connector;
                    break;
                default:
                    span[j++] = separator;
                    break;
            }
        }

        return StringPool.Shared.GetOrAdd(span[0..j]);
    }
}
