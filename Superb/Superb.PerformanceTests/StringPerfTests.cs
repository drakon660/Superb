using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Cysharp.Text;

namespace Superb.PerformanceTests;

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser()]
public class StringPerfTests
{
    [Benchmark]
    public string ZString_Concat_Test()
    {
        return ZString.Concat("1", "2", "3");
    }
    
    [Benchmark]
    public string ZString_Builder_Test()
    {
        using var builder = Cysharp.Text.ZString.CreateStringBuilder();
        builder.Append("1");
        builder.Append("2");
        builder.Append("3");
        return builder.ToString();
    }
    
    [Benchmark]
    public string String_Concat_Test()
    {
        return String.Concat("1", "2", "3");
    }
    
    [Benchmark]
    public string String_Concat_AsSpan_Test()
    {
        return String.Concat("1".AsSpan(), "2".AsSpan(), "3".AsSpan());
    }
}