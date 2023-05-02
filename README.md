```ps1
dotnet run -c Release -f net8.0 --filter '**'
```

## Results

```log
BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2), VM=Hyper-V
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=8.0.100-preview.4.23218.17
  [Host]     : .NET 8.0.0 (8.0.23.21704), X64 RyuJIT AVX2
  Job-POUQAN : .NET 8.0.0 (8.0.23.21704), X64 RyuJIT AVX2
```

| Method |    b |      Mean | Ratio | Code Size |
|------- |----- |----------:|------:|----------:|
| Before | True | 0.0270 ns |  1.00 |      30 B |
|  After | True | 0.0043 ns |  0.15 |      11 B |

```assembly
; Program.Before(System.Nullable`1<Boolean>)
; 	public bool Before(bool? b) => b == true;
; 	                               ^^^^^^^^^
       mov       [rsp+10],rdx
       movzx     eax,byte ptr [rsp+10]
       movzx     edx,byte ptr [rsp+11]
       cmp       dl,1
       sete      dl
       movzx     edx,dl
       movzx     eax,al
       and       eax,edx
       ret
; Total bytes of code 30
```

```assembly
; Program.After(System.Nullable`1<Boolean>)
; 	public bool After(bool? b) => b.GetValueOrDefault();
; 	                              ^^^^^^^^^^^^^^^^^^^^^
       mov       [rsp+10],rdx
       movzx     eax,byte ptr [rsp+11]
       ret
; Total bytes of code 11
```
