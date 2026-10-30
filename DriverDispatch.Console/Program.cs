using DriverDispatch.Core.Algorithms;
using DriverDispatch.Core.Models;

namespace DriverDispatch.Console;

internal static class Program
{
    private static void Main()
    {
        var drivers = new List<Driver>
        {
            new(1, 10, 10),
            new(2, 12, 10),
            new(3, 8, 10),
            new(4, 10, 12),
            new(5, 10, 8),
            new(6, 50, 50),
            new(7, 51, 50)
        };

        var order = new Order(10, 10);

        IDriverSearchAlgorithm[] algorithms =
        {
            new LinearSearchAlgorithm(),
            new PartialSortAlgorithm(),
            new GridSearchAlgorithm()
        };

        System.Console.WriteLine($"Order at ({order.X},{order.Y})");
        System.Console.WriteLine();

        foreach (var algo in algorithms)
        {
            var nearest = algo.FindNearestDrivers(drivers, order, 5);
            var ids = string.Join(", ", nearest.Select(d => d.Id));
            System.Console.WriteLine($"{algo.GetType().Name,-24} -> [{ids}]");
        }
    }
}