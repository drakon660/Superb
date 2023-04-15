using System.Text.Json;
using FluentAssertions;
using Superb.Core;
using Whatever;

namespace Superb.Tests;

public class SystemTextJsonTests
{
    [Fact]
    public void Test_Serialization()
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
        var value = JsonSerializer.Serialize(values);
        var hash = Flatter.Hash(value);
        var dict = new Dictionary<string, object> { { hash, value } };

        dict.Should().NotBeEmpty();
    }
}