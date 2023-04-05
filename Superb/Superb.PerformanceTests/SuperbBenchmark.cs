using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Whatever;

namespace Superb.PerformanceTests;

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser()]
public class SuperbBenchmark
{

    [Benchmark]
    public string Aggregate_String_Join_Select()
    {
         return Flatter.AggregateDictionaryToString(new Dictionary<string, object>
            {
                { "key1", 123 },
                { "key2", "hello" },
                { "key3", true },
                { "key4", 3.14 },
                { "key5", 'a' },
                { "key6", (byte) 255 },
                { "key7", (short) -32768 },
                { "key8", (ushort) 65535 },
                { "key9", (long) 1234567890 },
                { "key10", (ulong) 9876543210 },
                { "key11", (float) 1.23 },
                { "key12", (decimal) 3.14159 },
                { "key13", DateTime.Now },
                { "key14", TimeSpan.FromMinutes(30) },
                { "key15", Guid.NewGuid() },
                { "key16", 42 },
                { "key17", "world" },
                { "key18", false },
                { "key19", 2.71 },
                { "key20", 'b' },
                { "key21", (byte) 128 },
                { "key22", (short) 32767 },
                { "key23", (ushort) 12345 },
                { "key24", (long) -1234567890 },
                { "key25", (ulong) 9876543210 },
                { "key26", (float) 4.56 },
                { "key27", (decimal) 2.71828 },
                { "key28", DateTime.UtcNow },
                { "key29", TimeSpan.FromHours(1) },
                { "key30", Guid.NewGuid() },
                { "key31", 7 },
                { "key32", "foo" },
                { "key33", true },
                { "key34", 1.23 },
                { "key35", 'c' },
                { "key36", (byte) 64 },
                { "key37", (short) -12345 },
                { "key38", (ushort) 54321 },
                { "key39", (long) 9876543210 },
                { "key40", (ulong) 1234567890 },
                { "key41", (float) 7.89 },
                { "key42", (decimal) 1.41421 },
                { "key43", DateTime.Now.Date },
                { "key44", TimeSpan.FromDays(1) },
                { "key45", Guid.NewGuid() },
                { "key46", 99 },
                { "key47", "bar" },
                { "key48", false },
                { "key49", 4.56 },
                { "key50", 'd' },
            }
            );
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