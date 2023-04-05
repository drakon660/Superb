using FluentAssertions;
using Whatever;

namespace Superb.Tests;

public class UnitTest1
{
    [Fact]
    public void Test_Simple_Key()
    {
        object[] values = new object[]
        {
            new SampleRequest()
            {
                Value = "1",
                Fake = new SampleResponse()
                {
                    Val = 400,
                    Batman = new Batman()
                    {
                        Price = 666
                    }
                }
            },
            new SampleResponse()
            {
                Val = 1
            }
        };
        
        var dictionary = Flatter.ConvertToObjectDictionary(values);
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["SampleRequest.Value"] = "1",
            ["SampleRequest.Fake.Val"] = 400,
            ["SampleRequest.Fake.Batman.Price"] = 666,
            ["SampleResponse.Val"] = 1,
        });
    }
    
    [Fact]
    public void Test_Complex_Key()
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
                    },
                    Data = DateTime.Now
                }
            }
        };
        
        var dictionary = Flatter.ConvertToObjectDictionary(values);
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["SampleRequest.Fake.Val"] = 400,
            ["SampleRequest.Fake.Batman.Price"] = 666,
            ["SampleRequest.Fake.Data"] = DateTime.Now.ToString("yyyyMMdd")
        });
    }

    [Fact]
    public void Test_Simple_Key_Omit_Props()
    {
        object[] values = new object[]
        {
            new SampleRequest()
            {
                Value = "1",
                Fake = new SampleResponse()
                {
                    Val = 400,
                    Batman = new Batman()
                    {
                        Price = 666
                    }
                }
            },
            new SampleResponse()
            {
                Val = 1
            }
        };
        
        var dictionary = Flatter.ConvertToObjectDictionary(values,new []{ "SampleRequest.Fake.Batman.Price", "SampleRequest.Value"});
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["SampleRequest.Value"] = "1",
            ["SampleRequest.Fake.Batman.Price"] = 666,
        });
    }
    
    [Fact]
    public void Test_Aggregate_Omit_Props()
    {
        object[] values = new object[]
        {
            new SampleRequest()
            {
                Value = "1",
                Fake = new SampleResponse()
                {
                    Val = 400,
                    Batman = new Batman()
                    {
                        Price = 666
                    }
                }
            },
            new SampleResponse()
            {
                Val = 1
            }
        };
        
        var dictionary = Flatter.ConvertToObjectDictionary(values,new []{ "SampleRequest.Fake.Batman.Price", "SampleRequest.Value"});
        string compositeKey = Flatter.AggregateDictionaryToString(dictionary);
        
        compositeKey.Should().Be("SampleRequest.Value-1-SampleRequest.Fake.Batman.Price-666");
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["SampleRequest.Value"] = "1",
            ["SampleRequest.Fake.Batman.Price"] = 666,
        });
    }

    [Fact]
    public void Test_Array()
    {
        object[] values = new Object[]
        {
            new FilterRequest()
            {
                Filters = new [] { 
                    new Filter()
                {
                    Value2 = "value2",
                    Value1 = 555
                },
                    new Filter()
                    {
                        Value2 = "value8",
                        Value1 = 666778
                    }  
                }
            },
        };
        
        var dictionary = Flatter.ConvertToObjectDictionary(values);
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["[0].Filters.Value2"] = "value2",
            ["[0].Filters.Value1"] = 555,
            ["[1].Filters.Value2"] = "value8",
            ["[1].Filters.Value1"] = 666778,
        });
    }
    
    [Fact]
    public void Test_Array_No_All()
    {
        object[] values = new Object[]
        {
            new FilterRequest()
            {
                Filters = new [] { 
                    new Filter()
                    {
                        Value2 = "value2",
                        Value1 = 555
                    },
                    new Filter()
                    {
                        Value2 = "value8",
                        Value1 = 666778
                    }  
                }
            },
        };
        
        var dictionary = Flatter.ConvertToObjectDictionary(values, new string[]
        {
            "[1].Filters.Value2" 
        });
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["[1].Filters.Value2"] = "value8",
        });
    }
    
    [Fact]
    public void Test_Array_Null()
    {
        object[] values = new Object[]
        {
            new FilterRequest()
            {
                Filters = new Filter[]
                {
                    null
                }
            },
        };
        
        var dictionary = Flatter.ConvertToObjectDictionary(values);
        dictionary.Should().BeEmpty();
    }
    
    [Fact]
    public void Test_Hash()
    {
        //var key = "SampleRequest.Value-1-SampleRequest.Fake.Batman.Price-666";
        var hashCode = Flatter.Hash("SampleRequest.Value-1-SampleRequest.Fake.Batman.Price-666");

        hashCode.Should().Be("BD9F823FD6B4DA0DD88B54FB8B7CE68FE5776D3383D1E97326FF7DAC8A855AF8");
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
    public DateTime Data { get; set; }
}

public class FilterRequest
{
    public Filter[] Filters { get; set; }
}

public class Filter
{
    public int Value1 { get; set; }
    public string Value2 { get; set; }
}