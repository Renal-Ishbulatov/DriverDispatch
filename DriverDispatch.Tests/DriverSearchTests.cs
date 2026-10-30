using DriverDispatch.Core.Algorithms;
using DriverDispatch.Core.Models;
using NUnit.Framework;

namespace DriverDispatch.Tests;

[TestFixture]
public class DriverSearchTests
{
    private static readonly List<Driver> SampleDrivers = new()
    {
        new(1, 10, 10),
        new(2, 12, 10),
        new(3, 8, 10),
        new(4, 10, 12),
        new(5, 10, 8),
        new(6, 50, 50),
        new(7, 51, 50)
    };

    private static readonly Order SampleOrder = new(10, 10);

    private static IEnumerable<IDriverSearchAlgorithm> Algorithms()
    {
        yield return new LinearSearchAlgorithm();
        yield return new PartialSortAlgorithm();
        yield return new GridSearchAlgorithm();
    }

    [Test]
    public void AllAlgorithms_ReturnFiveNearest_WhenEnoughDrivers()
    {
        foreach (var algo in Algorithms())
        {
            var result = algo.FindNearestDrivers(SampleDrivers, SampleOrder, 5);

            Assert.That(result, Has.Count.EqualTo(5),
                $"{algo.GetType().Name} returned wrong count");

            var ids = result.Select(d => d.Id).OrderBy(id => id).ToList();
            Assert.That(ids, Is.EqualTo(new[] { 1, 2, 3, 4, 5 }),
                $"{algo.GetType().Name} returned wrong driver set");
        }
    }

    [Test]
    public void AllAlgorithms_ReturnAll_WhenFewerThanRequested()
    {
        var fewDrivers = new List<Driver> { new(1, 0, 0), new(2, 1, 1) };
        var order = new Order(0, 0);

        foreach (var algo in Algorithms())
        {
            var result = algo.FindNearestDrivers(fewDrivers, order, 5);
            Assert.That(result, Has.Count.EqualTo(2),
                $"{algo.GetType().Name} should return all available drivers");
        }
    }

    [Test]
    public void AllAlgorithms_ReturnEmpty_WhenNoDrivers()
    {
        foreach (var algo in Algorithms())
        {
            var result = algo.FindNearestDrivers(Array.Empty<Driver>(), SampleOrder, 5);
            Assert.That(result, Is.Empty,
                $"{algo.GetType().Name} should return empty for no drivers");
        }
    }

    [Test]
    public void AllAlgorithms_ReturnEmpty_WhenCountIsZeroOrNegative()
    {
        foreach (var algo in Algorithms())
        {
            Assert.That(algo.FindNearestDrivers(SampleDrivers, SampleOrder, 0),
                Is.Empty, $"{algo.GetType().Name} with count=0");

            Assert.That(algo.FindNearestDrivers(SampleDrivers, SampleOrder, -1),
                Is.Empty, $"{algo.GetType().Name} with count=-1");
        }
    }
}