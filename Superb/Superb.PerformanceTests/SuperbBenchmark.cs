using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Whatever;

namespace Superb.PerformanceTests;

[SimpleJob(RuntimeMoniker.Net60)]
public class SuperbBenchmark
{
    [Benchmark]
    public IReadOnlyDictionary<string, object> Flatter_Run2()  {
        
        object[] values = new object[]
        {
            new SampleRequest()
            {
                Value = null,
                Fake = new SampleResponse()
                {
                    Val = 400,
                    Batman = new Batman()
                    {
                        Price = 666
                    }
                }
            }
        };
        
        return Flatter2.ConvertToObjectDictionary(values);
    }

    [Benchmark]
    public IReadOnlyDictionary<string, object> Flatter_Run()
    {
        object[] values = new object[]
        {
            new SampleRequest()
            {
                Value = null,
                Fake = new SampleResponse()
                {
                    Val = 400,
                    Batman = new Batman()
                    {
                        Price = 666
                    }
                }
            }
        };
        
        return Flatter.ConvertToObjectDictionary(values);
    }
}

public class Batman
{
    public decimal Price { get; set; }
}
public class SampleRequest
{
    public string Value { get; set; }
    public SampleResponse Fake { get; set; }
}
public class SampleResponse
{
    public int Val { get; set; }
    public Batman Batman { get; set; }
}