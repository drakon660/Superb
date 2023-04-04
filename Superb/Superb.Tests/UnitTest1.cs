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