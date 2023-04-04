// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Superb.PerformanceTests;

//BenchmarkRunner.Run<SuperbBenchmark>();


string myString = "hello world";
string myString2 = "hello world";
int hashCode = myString.GetHashCode();
int hashCode2 = myString.GetHashCode();

Console.WriteLine(hashCode);
Console.WriteLine(hashCode2);

