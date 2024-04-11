![NuGet Version](https://img.shields.io/nuget/v/FastSlugger)

# What is it

  A blazingly fast slug creator. Create text that is easily read by humans and is url friendly.
  Typical use case would be to refer to resources by their name.  
  This can help both your users and search engines to categorize your exposed resources.

<p align=''>
  <img src="assets/images/snail.png">
</p>

Example slugs

| Input | Output |
|-------|--------|
| text.contains.!marks#: | textcontainsmarks |
| Hello Kønnìchiwa | hello-konnichiwa |
| some--LONG_space-- | some-long_space- |
| 👑 🌴 | crown-palm_tree |
| Привет | privet |


Behind the scenes, this uses the package `AnyAscii` slugger, which also transliterates.

This library then uses dashes to `separate-words` and makes all characters lowercase.  
Any punctuation, such as `#` or `.` are removed.  
Any consecutive dashes are squashed to a single dash.  
Any connector punctuation such as `‿` are converted to `_`.

# Installation

`dotnet add package FastSlugger`

# Usage

```csharp
using FastSlugger;

var text = "Create a new slug! Frøm ènglish#---txt";

// Output: "create-a-new-slug-from-english-txt"
var slug = Slug.Create(text);
```

You can configure the slugger by overriding the default parameter values for the connector character, the separator character and how long the slug should maximally be.

# Remarks
This library uses stack allocation if the slug length is less than 1000 characters, which can be seen by the benchmark result.

The AnyAscii library seems to be a huge custom lookup table.

Furthermore the string creation uses the high performance community toolkit library to minimize string allocation.

# Benchmark results

**System**  
BenchmarkDotNet v0.13.12, Ubuntu 23.04 (Lunar Lobster) WSL
AMD Ryzen 7 3700X, 1 CPU, 4 logical and 2 physical cores
.NET SDK 8.0.203
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2


**Result**  
| Method        | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| SlugifyCore   | 926.45 us | 4.951 us | 4.631 us | 28.97 |    0.13 |      - |  39.67 KB |        8.97 |
| FastSlugger   |  31.96 us | 0.093 us | 0.082 us |  1.00 |    0.00 |      - |   4.42 KB |        1.00 |
| SlugGenerator |  45.97 us | 0.111 us | 0.104 us |  1.44 |    0.00 | 0.1221 |  13.16 KB |        2.98 |

Outliers  
  SpeedComparisonBenchmarks.FastSlugger: Default -> 1 outlier  was  removed (32.78 us)

Package versions used:  
Slugify.Core v.2.0.2  
SlugGenerator v.4.0.1