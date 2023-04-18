namespace Superb.Core;

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

    public Item[] Item { get; set; }
}

public class Item
{
    public string Value { get; set; }
    public int Value2 { get; set; }
    public decimal Value3 { get; set; }
}

public class FilterRequest
{
    public Filter[] Filters { get; set; }
    public Item Item { get; set; }
}

public class Filter
{
    public int Value1 { get; set; }
    public string Value2 { get; set; }
    public MiniFilter[] Mini { get; set; }
}

public class MiniFilter
{
    public int TestValue { get; set; }
}