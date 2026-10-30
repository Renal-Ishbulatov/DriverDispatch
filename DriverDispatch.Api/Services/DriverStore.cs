using DriverDispatch.Core.Models;

namespace DriverDispatch.Api.Services;

public class DriverStore
{
    private readonly Dictionary<int, Driver> _drivers = new();
    private readonly object _lock = new();

    public bool TryGet(int id, out Driver driver)
    {
        lock (_lock) return _drivers.TryGetValue(id, out driver);
    }

    public bool IsCoordinateOccupied(int x, int y)
    {
        lock (_lock) return _drivers.Values.Any(d => d.X == x && d.Y == y);
    }

    public bool IsCoordinateOccupiedByOther(int id, int x, int y)
    {
        lock (_lock) return _drivers.Values.Any(d => d.Id != id && d.X == x && d.Y == y);
    }

    public void Set(Driver driver)
    {
        lock (_lock) _drivers[driver.Id] = driver;
    }

    public void Remove(int id)
    {
        lock (_lock) _drivers.Remove(id);
    }

    public IReadOnlyList<Driver> GetAll()
    {
        lock (_lock) return _drivers.Values.ToList();
    }
}