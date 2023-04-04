// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Superb.PerformanceTests;

BenchmarkRunner.Run<SuperbBenchmark>();

Console.WriteLine("Hello, World!");

