# ⏱️ Wizdle.Performance.Tests

## Latest

```
BenchmarkDotNet v0.15.0, Windows 11 (10.0.26100.4061/24H2/2024Update/HudsonValley)
Unknown processor
.NET SDK 9.0.204
  [Host]     : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
```

| Method                                  | WordLength |         Mean |        Error |       StdDev |
| --------------------------------------- | ---------- | -----------: | -----------: | -----------: |
| **WizdleEngine_WithOnlyCorrectLetters** | **1**      | **852.1 μs** |  **8.64 μs** |  **8.08 μs** |
| WizdleEngine_WithOnlyMisplacedLetters   | 1          |   2,399.8 μs |     46.25 μs |     56.80 μs |
| **WizdleEngine_WithOnlyCorrectLetters** | **2**      | **836.7 μs** | **15.57 μs** | **14.56 μs** |
| WizdleEngine_WithOnlyMisplacedLetters   | 2          |   2,547.4 μs |     49.98 μs |     63.21 μs |
| **WizdleEngine_WithOnlyCorrectLetters** | **3**      | **834.7 μs** | **15.62 μs** | **14.61 μs** |
| WizdleEngine_WithOnlyMisplacedLetters   | 3          |   2,563.7 μs |     50.58 μs |     80.22 μs |
| **WizdleEngine_WithOnlyCorrectLetters** | **4**      | **843.5 μs** | **12.74 μs** | **11.91 μs** |
| WizdleEngine_WithOnlyMisplacedLetters   | 4          |   2,608.6 μs |     51.00 μs |     68.09 μs |
| **WizdleEngine_WithOnlyCorrectLetters** | **5**      | **829.0 μs** |  **8.32 μs** |  **7.37 μs** |
| WizdleEngine_WithOnlyMisplacedLetters   | 5          |   2,544.8 μs |     49.56 μs |     60.87 μs |
