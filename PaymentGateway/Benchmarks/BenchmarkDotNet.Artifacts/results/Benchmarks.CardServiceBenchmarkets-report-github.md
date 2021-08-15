``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1165 (20H2/October2020Update)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.301
  [Host]     : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
  DefaultJob : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT


```
|             Method |     Mean |    Error |   StdDev | Rank |  Gen 0 | Allocated |
|------------------- |---------:|---------:|---------:|-----:|-------:|----------:|
|      MaskByReplace | 10.06 ns | 0.037 ns | 0.035 ns |    1 | 0.0067 |      56 B |
| MaskByRemoveInsert | 24.15 ns | 0.076 ns | 0.063 ns |    2 | 0.0124 |     104 B |
