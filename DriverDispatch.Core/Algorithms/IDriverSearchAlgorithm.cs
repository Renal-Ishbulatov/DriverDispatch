using DriverDispatch.Core.Models;

namespace DriverDispatch.Core.Algorithms;

public interface IDriverSearchAlgorithm
{
    /// <summary>
    /// Находит <paramref name="count"/> ближайших к заказу водителей.
    /// </summary>
    IReadOnlyList<Driver> FindNearestDrivers(
        IReadOnlyList<Driver> drivers,
        Order order,
        int count = 5);
}