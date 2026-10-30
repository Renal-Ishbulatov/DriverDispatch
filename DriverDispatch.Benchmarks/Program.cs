using BenchmarkDotNet.Running;

namespace DriverDispatch.Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<DriverSearchBenchmarks>(args: args);
    }
}