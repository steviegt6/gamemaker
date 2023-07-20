``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2965/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-XFMXML : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                                          Method |    Length |     Mean |     Error |    StdDev |   Median | Allocated |
|------------------------------------------------ |---------- |---------:|----------:|----------:|---------:|----------:|
|                     **ReadBytesMemoryAsMemoryRead** |   **1000000** | **270.1 ns** |  **58.72 ns** | **160.75 ns** | **200.0 ns** |     **600 B** |
|                  ReadBytesMemoryConstructorRead |   1000000 | 100.0 ns |   0.00 ns |   0.00 ns | 100.0 ns |     600 B |
|                    ReadBytesSpanConstructorRead |   1000000 | 169.9 ns |  50.60 ns | 143.55 ns | 100.0 ns |     600 B |
|            ReadBytesMemoryMarshalCreateSpanRead |   1000000 | 300.0 ns |  68.03 ns | 198.45 ns | 200.0 ns |     600 B |
| ReadBytesMemoryMarshalCreateFromPinnedArrayRead |   1000000 | 258.6 ns |  59.10 ns | 161.79 ns | 200.0 ns |     600 B |
|                     **ReadBytesMemoryAsMemoryRead** | **100000000** | **716.3 ns** | **118.62 ns** | **334.56 ns** | **600.0 ns** |     **600 B** |
|                  ReadBytesMemoryConstructorRead | 100000000 | 838.8 ns | 150.97 ns | 440.39 ns | 600.0 ns |     600 B |
|                    ReadBytesSpanConstructorRead | 100000000 | 721.6 ns | 113.58 ns | 329.53 ns | 600.0 ns |     600 B |
|            ReadBytesMemoryMarshalCreateSpanRead | 100000000 | 743.3 ns | 120.94 ns | 350.86 ns | 600.0 ns |     600 B |
| ReadBytesMemoryMarshalCreateFromPinnedArrayRead | 100000000 | 860.8 ns | 184.32 ns | 534.74 ns | 600.0 ns |     600 B |
