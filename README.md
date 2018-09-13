### Overview

This adds a [Jace.NET](https://github.com/pieterderycke/Jace) transform to Transformalize.  It is a plug-in compatible with Transformalize 0.3.7-beta.

Build the Autofac project and put it's output into Transformalize's *plugins* folder.

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

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.16299.251 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742188 Hz, Resolution=364.6723 ns, Timer=TSC
  [Host]       : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.2633.0
  LegacyJitX64 : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit LegacyJIT/clrjit-v4.7.2633.0;compatjit-v4.7.2633.0

Job=LegacyJitX64  Jit=LegacyJit  Platform=X64  
Runtime=Clr  

```
|             Method |     Mean |    Error |   StdDev | Scaled |
|------------------- |---------:|---------:|---------:|-------:|
|        &#39;5000 rows&#39; | 449.7 ms | 3.014 ms | 2.672 ms |   1.00 |
| &#39;5000 rows 1 jace&#39; | 463.2 ms | 2.760 ms | 2.582 ms |   1.03 |
