using FluentAssertions;
using Superb.Core;
using Whatever;

namespace Superb.Tests;

public class FlatterUnitTests
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

        var dictionary = Flatter.ConvertToObjectDictionary(values);
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

        var dictionary = Flatter.ConvertToObjectDictionary(values);
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
                    }
                }
            },
        };

        var dictionary = Flatter.ConvertToObjectDictionary(values);
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["FilterRequest.Filters[0].Value2"] = "value2",
            ["FilterRequest.Filters[0].Value1"] = 555,
            ["FilterRequest.Filters[1].Value2"] = "value8",
            ["FilterRequest.Filters[1].Value1"] = 666778,
        });
    }
    
    [Fact]
    public void Test_Array_And_Object()
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
                        Value1 = 666778,
                        Mini = new [] { new MiniFilter() { TestValue = 606 } }
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
                }
            }
        };

        var dictionary = Flatter.ConvertToObjectDictionary(values);
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["FilterRequest.Filters[0].Value1"] = 555,
            ["FilterRequest.Filters[0].Value2"] = "value2",
            ["FilterRequest.Filters[1].Value1"] = 666778,
            ["FilterRequest.Filters[1].Value2"] = "value8",
            ["FilterRequest.Filters[1].Mini[0].TestValue"]=606,
            ["FilterRequest.Item.Value"] = "devil",
            ["FilterRequest.Item.Value2"] = 0,
            ["FilterRequest.Item.Value3"] = 0,
            ["SampleResponse.Val"] = 15,
            ["SampleResponse.Batman.Price"] = 963,
        });
    }

    [Fact]
    public void Test_Array_No_All()
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
                    }
                }
            },
        };

        var dictionary = Flatter.ConvertToObjectDictionary(values);
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["[1].Filters.Value2"] = "value8",
        });
    }

    [Fact]
    public void Test_Array_On_Top()
    {
        object[] values = new Object[]
        {
            new[]
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
                }
            }
        };

        var dictionary = Flatter.ConvertToObjectDictionary(values, true);
        dictionary.Should().BeEquivalentTo(new Dictionary<string, object>()
        {
            ["Filter[0].Value2"] = "value2",
            ["Filter[0].Value1"] = 555,
            ["Filter[1].Value2"] = "value8",
            ["Filter[1].Value1"] = 666778,
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

