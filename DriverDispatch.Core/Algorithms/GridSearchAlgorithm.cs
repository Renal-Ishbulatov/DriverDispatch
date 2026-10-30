using DriverDispatch.Core.Models;

namespace DriverDispatch.Core.Algorithms;

/// <summary>
/// Использует тот факт, что карта — прямоугольная сетка, и в клетке не более одного водителя.
/// Строит индекс «координата → водитель» и обходит кольца вокруг заказа,
/// пока не наберёт count водителей.
/// </summary>
public class GridSearchAlgorithm : IDriverSearchAlgorithm
{
    public IReadOnlyList<Driver> FindNearestDrivers(
        IReadOnlyList<Driver> drivers,
        Order order,
        int count = 5)
    {
        if (drivers.Count == 0 || count <= 0)
            return Array.Empty<Driver>();

        var grid = new Dictionary<(int X, int Y), Driver>(drivers.Count);
        foreach (var driver in drivers)
            grid[(driver.X, driver.Y)] = driver;

        var result = new List<Driver>(count);
        int maxRadius = drivers.Count + 1;

        for (int radius = 0; radius <= maxRadius && result.Count < count; radius++)
        {
            for (int dx = -radius; dx <= radius && result.Count < count; dx++)
            {
                for (int dy = -radius; dy <= radius && result.Count < count; dy++)
                {
                    if (Math.Abs(dx) != radius && Math.Abs(dy) != radius)
                        continue;

                    int x = order.X + dx;
                    int y = order.Y + dy;

                    if (grid.TryGetValue((x, y), out var driver))
                        result.Add(driver);
                }
            }
        }

        return result;
    }
}