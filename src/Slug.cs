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
    /// <returns>A slug</returns>
    public static string Create(string input)
    {
        var transliterate = input.Transliterate();
        return ToLowercaseAndHyphenSeparator(transliterate);
    }

    private static string ToLowercaseAndHyphenSeparator(string text)
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
                    span[j++] = '_';
                    break;
                default:
                    span[j++] = '-';
                    break;
            }
        }

        return StringPool.Shared.GetOrAdd(span[0..j]);
    }
}
