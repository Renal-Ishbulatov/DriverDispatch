using BenchmarkDotNet.Attributes;
using DriverDispatch.Core.Algorithms;
using DriverDispatch.Core.Models;

namespace DriverDispatch.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 5)]
public class DriverSearchBenchmarks
{
    private List<Driver> _drivers = null!;
    private Order _order;

    private readonly IDriverSearchAlgorithm _linear = new LinearSearchAlgorithm();
    private readonly IDriverSearchAlgorithm _partial = new PartialSortAlgorithm();
    private readonly IDriverSearchAlgorithm _grid = new GridSearchAlgorithm();

    [Params(1_000, 10_000, 100_000)]
    public int DriverCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _drivers = new List<Driver>(DriverCount);
        var used = new HashSet<(int, int)>();

        while (_drivers.Count < DriverCount)
        {
            int x = random.Next(0, 1000);
            int y = random.Next(0, 1000);
            if (used.Add((x, y)))
                _drivers.Add(new Driver(_drivers.Count, x, y));
        }

        _order = new Order(500, 500);
    }

    [Benchmark]
    public IReadOnlyList<Driver> Linear() => _linear.FindNearestDrivers(_drivers, _order, 5);

    [Benchmark]
    public IReadOnlyList<Driver> PartialSort() => _partial.FindNearestDrivers(_drivers, _order, 5);

    [Benchmark]
    public IReadOnlyList<Driver> Grid() => _grid.FindNearestDrivers(_drivers, _order, 5);
}