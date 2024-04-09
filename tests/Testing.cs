using FluentAssertions;
using Xunit.Categories;

namespace FastSlugger.Tests;

[UnitTest]
public class Testing
{
    [Fact]
    public void Space_should_turn_into_dashes()
    {
        // Arrange
        var text = "this is a text with spaces";

        // Act
        var slug = Slug.Create(text);

        // Assert
        slug.Should().Be("this-is-a-text-with-spaces");
    }

    [Fact]
    public void Diacritics_should_be_normalized()
    {
        // Arrange
        var text = "nò Hàbla español";

        // Act
        var slug = Slug.Create(text);

        // Assert
        slug.Should().Be("no-habla-espanol");
    }

    [Fact]
    public void Uppercase_should_be_turned_to_lowercase()
    {
        // Arrange
        var text = "YELLING IS NOT ALLOWED";

        // Act
        var slug = Slug.Create(text);

        // Assert
        slug.Should().Be("yelling-is-not-allowed");
    }

    [Fact]
    public void Multiline_should_result_in_one_line()
    {
        // Arrange
        var text = """
        Привет
        thîs
        cöntains
        æøå letters
        And newlines
        """;

        // Act
        var slug = Slug.Create(text);

        // Assert
        slug.Should().Be("privet-this-contains-aeoa-letters-and-newlines");
    }

    [Fact]
    public void Punctuations_should_be_stripped()
    {
        // Arrange
        var text = "text.contains.!marks#:";

        // Act
        var slug = Slug.Create(text);

        // Assert
        slug.Should().Be("textcontainsmarks");
    }

    [Fact]
    public void Emoji_should_be_transliterated()
    {
        // Arrange
        var text = "👑 🌴";

        // Act
        var slug = Slug.Create(text);

        // Assert
        slug.Should().Be("crown-palm_tree");
    }
}