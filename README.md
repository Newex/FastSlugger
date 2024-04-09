# What is it
A slug creator.

Behind the scenes, this uses the package `AnyAscii` slugger. But the output is refined further to replace spaces with dashes and non alphanumeric characters with empty characters.

# Installation

`dotnet add package FastSlugger`

# 

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