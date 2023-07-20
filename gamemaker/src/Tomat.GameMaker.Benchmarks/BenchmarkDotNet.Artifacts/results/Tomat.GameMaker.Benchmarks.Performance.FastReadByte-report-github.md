``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2965/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-YWDNKQ : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|      Method |    Length |       Mean |     Error |    StdDev |     Median |   Allocated |
|------------ |---------- |-----------:|----------:|----------:|-----------:|------------:|
|   **ArrayRead** |   **1000000** |   **1.148 ms** | **0.0461 ms** | **0.1360 ms** |   **1.219 ms** |   **977.17 KB** |
| PointerRead |   1000000 |   1.636 ms | 0.0472 ms | 0.1386 ms |   1.695 ms |   977.17 KB |
|   **ArrayRead** | **100000000** | **100.053 ms** | **1.4602 ms** | **1.2945 ms** |  **99.740 ms** | **97656.86 KB** |
| PointerRead | 100000000 | 145.717 ms | 1.4716 ms | 1.3765 ms | 145.100 ms | 97656.86 KB |
