using FluentAssertions;

namespace Superb.Tests;

public class PostQueryTests
{
    [Fact]
    public void Test_Simple_Post_Query()
    {
        var wrapper = new ClientWrapper();
        var values =wrapper.FindData(new Request());
        values.Users.Should().NotBeEmpty();
    }
}