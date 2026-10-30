using DriverDispatch.Core.Models;

namespace DriverDispatch.Core.Algorithms;

/// <summary>
/// Наивный алгоритм: вычисляет расстояние до всех водителей
/// и сортирует весь список. Сложность O(n log n).
/// </summary>
public class LinearSearchAlgorithm : IDriverSearchAlgorithm
{
    public IReadOnlyList<Driver> FindNearestDrivers(
        IReadOnlyList<Driver> drivers,
        Order order,
        int count = 5)
    {
        if (drivers.Count == 0 || count <= 0)
            return Array.Empty<Driver>();

        var candidates = new List<(Driver Driver, long DistanceSquared)>(drivers.Count);

        foreach (var driver in drivers)
        {
            long dx = driver.X - order.X;
            long dy = driver.Y - order.Y;
            long distanceSquared = dx * dx + dy * dy;
            candidates.Add((driver, distanceSquared));
        }

        candidates.Sort((a, b) => a.DistanceSquared.CompareTo(b.DistanceSquared));

        int take = Math.Min(count, candidates.Count);
        var result = new Driver[take];
        for (int i = 0; i < take; i++)
            result[i] = candidates[i].Driver;

        return result;
    }
}