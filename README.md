### Overview

This adds a [Jace.NET](https://github.com/pieterderycke/Jace) 
transform to Transformalize.

### Usage

```xml
<cfg name="Test">
    <entities>
        <add name="Test">
            <rows>
                <add number1="3" number2="7" />
            </rows>
            <fields>
                <add name="number1" type="double" />
                <add name="number2" type="double" />
            </fields>
            <calculated-fields>
                <add name="jaced" t='jace(number1*number2)' />
            </calculated-fields>
        </add>
    </entities>
</cfg>
```
output:

```bash
> tfl.exe -a readme.xml
number1,number2,jaced
3,7,21
```

### Benchmark

``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.407 (1803/April2018Update/Redstone4)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742192 Hz, Resolution=364.6718 ns, Timer=TSC
  [Host]       : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3221.0
  LegacyJitX64 : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit LegacyJIT/clrjit-v4.7.3221.0;compatjit-v4.7.3221.0

Job=LegacyJitX64  Jit=LegacyJit  Platform=X64  
Runtime=Clr  

```
|             Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
|------------------- |---------:|---------:|---------:|------:|--------:|
|        &#39;5000 rows&#39; | 420.0 ms | 8.069 ms | 9.293 ms |  1.00 |    0.00 |
| &#39;5000 rows 1 jace&#39; | 425.5 ms | 3.760 ms | 3.518 ms |  1.02 |    0.03 |
