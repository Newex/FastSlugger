using BenchmarkDotNet.Attributes;
using Bogus;
using SlugGenerator;

namespace FastSlugger.Benchmarks.Speed;

[MemoryDiagnoser]
public class SpeedComparisonBenchmarks
{
    private readonly string text;
    public SpeedComparisonBenchmarks()
    {
        Randomizer.Seed = new Random(891236);
        var faker = new Faker();
        // text = faker.Random.Words(200) + faker.Random.String(100);
        text = faker.Random.Words(200);
    }

    [Benchmark]
    public string SlugifyCore()
    {
        var slugger = new Slugify.SlugHelper();
        return slugger.GenerateSlug(text);
    }

    [Benchmark(Baseline = true)]
    public string FastSlugger()
    {
        return Slug.Create(text);
    }

    [Benchmark]
    public string SlugGenerator()
    {
        return text.GenerateSlug();
    }
}
