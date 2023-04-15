using System.Runtime.InteropServices;
using Castle.DynamicLinqQueryBuilder;

namespace Superb.Tests;

public class PostQuery
{
    
}

public class Client //SOAP client
{
    public Data GetData(Request request)
    {
        var result = new Data() {  }; 
        return result;
    }

}

public class ClientWrapper //wrapper e.g controller, service etc
{
    private readonly Client _client = new Client();
    
    public Data FindData(Request request)
    {
        var result = _client.GetData(request); //if a cache apply get data from cache
        var postQueryBuilder = new UserPostQueryBuilder();
        result.Users = result.Users.BuildQuery(postQueryBuilder.Build(request));
        return result;
    }
}

public abstract class PostCacheQueryBuilder<T>
{
    public abstract IFilterRule Build(T t);
}

public sealed class UserPostQueryBuilder : PostCacheQueryBuilder<Request>
{
    public override IFilterRule Build(Request request)
    {
       return new QueryBuilderFilterRule()
        {
            Condition = "or",
            Rules = new List<QueryBuilderFilterRule>()
            {
                new QueryBuilderFilterRule()
                {
                    Condition = "and",
                    Field = "Name",
                    Id = "Name",
                    Input = "NA",
                    Operator = "equal",
                    Type = "string",
                    Value = new [] { request.Name }
                },
                
                // new QueryBuilderFilterRule()
                // {
                //     Condition = "and",
                //     Field = "Name",
                //     Id = "Name",
                //     Input = "NA",
                //     Operator = "equal",
                //     Type = "string",
                //     Value = new [] { "Bob" }
                // }
            }
        };
    }
}

public record Request
{
    public string Name { get; set; } // post cache filter
    public DateOnly From { get; set; } // part of cache key
    public DateOnly To { get; set; } // part of cache key
}

public class Data
{
    public IEnumerable<User> Users { get; set; } = new List<User>
    {
        new User { Name = "John", FamilyName = "Doe" },
        new User { Name = "Jane", FamilyName = "Doe" },
        new User { Name = "Bob", FamilyName = "Smith" },
        new User { Name = "Alice", FamilyName = "Jones" },
        new User { Name = "Chris", FamilyName = "Johnson" },
        new User { Name = "Emily", FamilyName = "Taylor" }
    };
}

public class User
{
    public string Name { get; set; }
    public string FamilyName { get; set; }
}