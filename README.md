![NuGet Version](https://img.shields.io/nuget/v/FastSlugger)

<p align=''>
  <img src="assets/images/snail.png">
</p>

# What is it

  🔥 A blazingly fast slug creator. Create text that is easily read by humans and is url friendly.
  Typical use case would be to refer to resources by their name.  
  This can help both your users and search engines to categorize your exposed resources.

Example slugs

| Input | Output |
|-------|--------|
| text.contains.!marks#: | textcontainsmarks |
| Hello Kønnìchiwa | hello-konnichiwa |
| some--LONG_space-- | some-long_space- |
| 👑 🌴 | crown-palm_tree |
| Привет | privet |


Behind the scenes, this uses the package `AnyAscii` slugger, which also transliterates.

By default FastSlugger will do the following:
- [x] use (`-`) dashes to `separate-words`
- [x] truncate any consecutive dashes to a single dash
- [x] make all characters lowercase
- [x] remove any punctuation, such as `!`, `/`, `#` or `.`
- [x] convert any connector punctuation such as `‿` to `_`.
- [x] truncate any consecutive connector punctuation to a single underscore `_`


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

# Options

The `Slug.Create` method takes 4 optional parameters that can configure the generated slug.

```csharp
// Signature:
string Create(string input, char connector, char separator, char? punctuation, int? max);
```

These are:
- `connector` - which changes the character used for underscore. Default value is `_`
- `separator` - which changes the character used for spaces. Default value is `-`
- `punctuation` - which changes the character used for punctuations. Default value is removal of the character
- `max` - defines the maximum length of the slug. The slug can be smaller than the max value. Default is unlimited length


# Remarks
This library uses stack allocation if the slug length is less than 1000 characters, which can be seen by the benchmark result.

The AnyAscii library seems to be a huge custom lookup table.

Furthermore the string creation uses the high performance community toolkit library to minimize string allocation.

# Benchmark results

**Notes on benchmarking**  
There are 2 ways to test the max size limit of a slug.

a) Cut the input text to the max size before creating the slug.  
b) Cut the slug to the max size after creation.

Option a - is the most efficient method.  
Option b - ensures correctness by making the output no longer than the max limit

`FastSlugger` does option a, natively, then during lowercase and hyphen construction phase, the constructed slug will stop when max size is reached (option b). Best of both worlds. Neither `Slugify.Core` or `SlugGenerator` has the native option to limit max output size.

## Test scenario
Well-formed UTF16 string with 200 random words as input, with no additional configuration, i.e. no max size limit or special character handling. See benchmacks folder for specific setup.

Why this scenario? Because otherwise there would be no data for `Slugify.Core` since it cannot run on ill-formed UTF16 strings.

**System**  

```
BenchmarkDotNet v0.13.12, Ubuntu 23.04 (Lunar Lobster) WSL
AMD Ryzen 7 3700X, 1 CPU, 4 logical and 2 physical cores
.NET SDK 8.0.203
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
```


**Result**  
| Method        | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| SlugifyCore   | 924.23 us | 1.760 us | 1.560 us | 28.96 |    0.12 |      - |  39.67 KB |        8.97 |
| FastSlugger   |  31.92 us | 0.127 us | 0.113 us |  1.00 |    0.00 |      - |   4.42 KB |        1.00 |
| SlugGenerator |  45.27 us | 0.174 us | 0.163 us |  1.42 |    0.01 | 0.1221 |  13.16 KB |        2.98 |

Outliers:  
- SpeedComparisonBenchmarks.SlugifyCore: Default -> 1 outlier  was  removed (928.85 us)  
- SpeedComparisonBenchmarks.FastSlugger: Default -> 1 outlier  was  removed (32.30 us)

Package versions used:  
- Slugify.Core v.2.0.2  
- SlugGenerator v.4.0.1

## Findings
- **FastSlugger** handles ill-formed strings and respects max size limit for slug. Is conservative with heap allocation, and uses shared string caching making it consistently first in memory footprint and in normal slug creation use case.

- **SlugGenerator** seems to be a viable alternative, can be ~1.6x faster than FastSlugger in some benchmark runs, when input is constructed with ill-formed UTF strings - though I cannot speak to the validity of the output slug.

- **Slugify.Core** is consistently last place in my benchmarks, in both speed and memory footprint, cannot handle ill-formed UTF strings.