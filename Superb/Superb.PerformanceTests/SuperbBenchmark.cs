using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Superb.Core;
using Whatever;

namespace Superb.PerformanceTests;

//[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser()]
public class SuperbBenchmark
{
    [Benchmark]
    public IReadOnlyDictionary<string, object> Test_Key_Creating_Json()
    {
        object[] values = new Object[]
        {
            new FilterRequest()
            {
                Filters = new[]
                {
                    new Filter()
                    {
                        Value2 = "value2",
                        Value1 = 555
                    },
                    new Filter()
                    {
                        Value2 = "value8",
                        Value1 = 666778
                    },
                    // Add 15 more filters below
                    new Filter()
                    {
                        Value2 = "value3",
                        Value1 = 12345
                    },
                    new Filter()
                    {
                        Value2 = "value4",
                        Value1 = 67890
                    },
                    new Filter()
                    {
                        Value2 = "value5",
                        Value1 = 13579
                    },
                    new Filter()
                    {
                        Value2 = "value6",
                        Value1 = 24680
                    },
                    new Filter()
                    {
                        Value2 = "value7",
                        Value1 = 98765
                    },
                    new Filter()
                    {
                        Value2 = "value8",
                        Value1 = 43210
                    },
                    new Filter()
                    {
                        Value2 = "value9",
                        Value1 = 11111
                    },
                    new Filter()
                    {
                        Value2 = "value10",
                        Value1 = 22222
                    },
                    new Filter()
                    {
                        Value2 = "value11",
                        Value1 = 33333
                    },
                    new Filter()
                    {
                        Value2 = "value12",
                        Value1 = 44444
                    },
                    new Filter()
                    {
                        Value2 = "value13",
                        Value1 = 55555
                    },
                    new Filter()
                    {
                        Value2 = "value14",
                        Value1 = 66666
                    },
                    new Filter()
                    {
                        Value2 = "value15",
                        Value1 = 77777
                    },
                    new Filter()
                    {
                        Value2 = "value16",
                        Value1 = 88888
                    }
                },
                Item = new Item()
                {
                    Value = "devil"
                }
            },
            new SampleResponse()
            {
                Val = 15,
                Batman = new Batman()
                {
                    Price = 963
                },
                Data = new DateTime(2050,11,12),
                Item = new []{ new Item()
                {
                    Value = "asdsadsadasdsad",
                    Value2 = 899779784,
                    Value3 = 355346545675687678687M
                }}
            }
        };

        var value = JsonSerializer.Serialize(values);
        var hash = Flatter.Hash(value);
        var dict = new Dictionary<string, object> { { hash, value } };
        return dict;
    }
    
     [Benchmark]
    public IReadOnlyDictionary<string, object> Test_Key_Creating_Flatter()
    {
         object[] values = new Object[]
        {
            new FilterRequest()
            {
                Filters = new[]
                {
                    new Filter()
                    {
                        Value2 = "value2",
                        Value1 = 555
                    },
                    new Filter()
                    {
                        Value2 = "value8",
                        Value1 = 666778
                    },
                    // Add 15 more filters below
                    new Filter()
                    {
                        Value2 = "value3",
                        Value1 = 12345
                    },
                    new Filter()
                    {
                        Value2 = "value4",
                        Value1 = 67890
                    },
                    new Filter()
                    {
                        Value2 = "value5",
                        Value1 = 13579
                    },
                    new Filter()
                    {
                        Value2 = "value6",
                        Value1 = 24680
                    },
                    new Filter()
                    {
                        Value2 = "value7",
                        Value1 = 98765
                    },
                    new Filter()
                    {
                        Value2 = "value8",
                        Value1 = 43210
                    },
                    new Filter()
                    {
                        Value2 = "value9",
                        Value1 = 11111
                    },
                    new Filter()
                    {
                        Value2 = "value10",
                        Value1 = 22222
                    },
                    new Filter()
                    {
                        Value2 = "value11",
                        Value1 = 33333
                    },
                    new Filter()
                    {
                        Value2 = "value12",
                        Value1 = 44444
                    },
                    new Filter()
                    {
                        Value2 = "value13",
                        Value1 = 55555
                    },
                    new Filter()
                    {
                        Value2 = "value14",
                        Value1 = 66666
                    },
                    new Filter()
                    {
                        Value2 = "value15",
                        Value1 = 77777
                    },
                    new Filter()
                    {
                        Value2 = "value16",
                        Value1 = 88888
                    }
                },
                Item = new Item()
                {
                    Value = "devil"
                }
            },
            new SampleResponse()
            {
                Val = 15,
                Batman = new Batman()
                {
                    Price = 963
                },
                Data = new DateTime(2050,11,12),
                Item = new []{ new Item()
                {
                    Value = "asdsadsadasdsad",
                    Value2 = 899779784,
                    Value3 = 355346545675687678687M
                }}
            }
        };

         return Flatter.ConvertToObjectDictionary(values);
    }
}