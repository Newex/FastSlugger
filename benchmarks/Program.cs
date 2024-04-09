// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using FastSlugger.Benchmarks.Speed;

var summary = BenchmarkRunner.Run<SpeedComparisonBenchmarks>();