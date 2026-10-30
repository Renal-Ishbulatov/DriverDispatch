using DriverDispatch.Core.Models;

namespace DriverDispatch.Core.Algorithms;

/// <summary>
/// Держим max-heap размера count. В корне — самый дальний из текущих «лучших».
/// Если новый водитель ближе — выкидываем корень и добавляем нового.
/// Сложность O(n log k), k = count.
/// </summary>
public class PartialSortAlgorithm : IDriverSearchAlgorithm
{
    public IReadOnlyList<Driver> FindNearestDrivers(
        IReadOnlyList<Driver> drivers,
        Order order,
        int count = 5)
    {
        if (drivers.Count == 0 || count <= 0)
            return Array.Empty<Driver>();

        // В .NET PriorityQueue — min-heap. Инвертируем приоритет, чтобы получить max-heap.
        var heap = new PriorityQueue<Driver, long>(count);

        foreach (var driver in drivers)
        {
            long dx = driver.X - order.X;
            long dy = driver.Y - order.Y;
            long distanceSquared = dx * dx + dy * dy;
            long priority = -distanceSquared;

            if (heap.Count < count)
            {
                heap.Enqueue(driver, priority);
            }
            else
            {
                heap.TryPeek(out _, out long worstPriority);
                if (priority > worstPriority)
                {
                    heap.Dequeue();
                    heap.Enqueue(driver, priority);
                }
            }
        }

        var result = new List<Driver>(heap.Count);
        while (heap.TryDequeue(out var driver, out _))
            result.Add(driver);

        result.Reverse();
        return result;
    }
}