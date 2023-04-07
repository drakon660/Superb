using System.Runtime.InteropServices;

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
        result.Users = result.Users.Where(x => x.Name == request.Name); //filter data after cache
        return result;
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
    public IEnumerable<User> Users { get; set; }
}

public class User
{
    public string Name { get; set; }
    public string FamilyName { get; set; }
}