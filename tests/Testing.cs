using Bogus;
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

    [Fact]
    public void Long_bogus_string_should_not_be_cutoff_if_max_is_greater_than_length()
    {
        // Arrange
        var faker = new Faker();
        var mumboJumbo = faker.Random.String(20);

        // Act
        var slug = Slug.Create(mumboJumbo, max: 100);

        // Assert
        slug.Length.Should().BeLessThanOrEqualTo(100);
    }

    [Fact]
    public void Long_bogus_string_should_be_cutoff_if_max_is_less_than_length()
    {
        // Arrange
        var faker = new Faker();
        var mumboJumbo = faker.Random.String(500);

        // Act
        var slug = Slug.Create(mumboJumbo, max: 20);

        // Assert
        slug.Length.Should().Be(20);
    }

    [Fact]
    public void Simple_slug()
    {
        // Arrange
        var text = "Create a new slug! Frøm ènglish#---txt";

        // Act
        var slug = Slug.Create(text);

        // Assert
        slug.Should().Be("create-a-new-slug-from-english-txt");
    }
}
