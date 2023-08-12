``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2965/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  Job-IFQIMA : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                         Method |    Length |       Mean |     Error |    StdDev | Allocated |
|------------------------------- |---------- |-----------:|----------:|----------:|----------:|
|               **BitConverterRead** |   **1000000** |   **2.192 ms** | **0.0564 ms** | **0.1638 ms** |   **1.91 MB** |
|                      ArrayRead |   1000000 |   2.449 ms | 0.0696 ms | 0.1870 ms |   1.91 MB |
|                    PointerRead |   1000000 |   1.873 ms | 0.0500 ms | 0.1468 ms |   1.91 MB |
|                   UnsafeAsRead |   1000000 |   1.500 ms | 0.0691 ms | 0.2005 ms |   1.91 MB |
|        UnsafeReadUnalignedRead |   1000000 |   1.352 ms | 0.0444 ms | 0.1294 ms |   1.91 MB |
|                UnsafeReadURead |   1000000 |   2.206 ms | 0.0675 ms | 0.1790 ms |   1.91 MB |
|             GenericPointerRead |   1000000 |   1.863 ms | 0.0553 ms | 0.1605 ms |   1.91 MB |
|            GenericUnsafeAsRead |   1000000 |   1.356 ms | 0.0499 ms | 0.1464 ms |   1.91 MB |
| GenericUnsafeReadUnalignedRead |   1000000 |   1.343 ms | 0.0396 ms | 0.1147 ms |   1.91 MB |
|          GenericUnsafeReadRead |   1000000 |   1.867 ms | 0.0592 ms | 0.1746 ms |   1.91 MB |
|               **BitConverterRead** | **100000000** | **174.914 ms** | **3.2824 ms** | **3.0704 ms** | **190.74 MB** |
|                      ArrayRead | 100000000 | 196.059 ms | 3.6654 ms | 3.4286 ms | 190.74 MB |
|                    PointerRead | 100000000 | 135.738 ms | 1.3176 ms | 1.1680 ms | 190.74 MB |
|                   UnsafeAsRead | 100000000 |  91.489 ms | 1.1631 ms | 0.9713 ms | 190.74 MB |
|        UnsafeReadUnalignedRead | 100000000 |  91.442 ms | 1.4212 ms | 1.3294 ms | 190.74 MB |
|                UnsafeReadURead | 100000000 | 172.539 ms | 3.2523 ms | 3.0422 ms | 190.74 MB |
|             GenericPointerRead | 100000000 | 136.819 ms | 2.4502 ms | 2.0460 ms | 190.74 MB |
|            GenericUnsafeAsRead | 100000000 |  92.382 ms | 1.1805 ms | 1.1042 ms | 190.74 MB |
| GenericUnsafeReadUnalignedRead | 100000000 |  91.735 ms | 1.2125 ms | 1.1341 ms | 190.74 MB |
|          GenericUnsafeReadRead | 100000000 | 136.216 ms | 1.4120 ms | 1.2517 ms | 190.74 MB |
