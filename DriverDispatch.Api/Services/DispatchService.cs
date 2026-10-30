using DriverDispatch.Api.Contracts;
using DriverDispatch.Core.Algorithms;
using DriverDispatch.Core.Models;

namespace DriverDispatch.Api.Services;

public class DispatchService : IDispatchService
{
    private readonly DriverStore _store;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DispatchService> _logger;

    public DispatchService(
        DriverStore store,
        IHttpClientFactory httpClientFactory,
        ILogger<DispatchService> logger)
    {
        _store = store;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<DriverAssignmentResponse?> AssignDriverAsync(int orderX, int orderY)
    {
        var allDrivers = _store.GetAll();
        if (allDrivers.Count == 0)
            return null;

        // Лучший алгоритм из части 1
        var algorithm = new PartialSortAlgorithm();
        var order = new Order(orderX, orderY);
        var nearest = algorithm.FindNearestDrivers(allDrivers, order, 5);

        if (nearest.Count == 0)
            return null;

        // Случайный индекс: внешний API или fallback
        int index = await GetRandomIndexAsync(nearest.Count);
        var chosen = nearest[index];

        var route = BuildRoute(chosen.X, chosen.Y, orderX, orderY);

        return new DriverAssignmentResponse
        {
            DriverId = chosen.Id,
            X = chosen.X,
            Y = chosen.Y,
            RouteLength = route.Count - 1,
            Route = route
        };
    }

    private async Task<int> GetRandomIndexAsync(int maxExclusive)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"http://www.randomnumberapi.com/api/v1.0/random?min=0&max={maxExclusive - 1}&count=1";
            var response = await client.GetFromJsonAsync<int[]>(url);

            if (response is { Length: > 0 })
                return Math.Clamp(response[0], 0, maxExclusive - 1);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RandomNumberAPI недоступен, используем System.Random");
        }

        return Random.Shared.Next(0, maxExclusive);
    }

    private static List<CoordinateDto> BuildRoute(int startX, int startY, int targetX, int targetY)
    {
        var route = new List<CoordinateDto>();
        int x = startX, y = startY;

        while (x != targetX)
        {
            x += Math.Sign(targetX - x);
            route.Add(new CoordinateDto { X = x, Y = y });
        }

        while (y != targetY)
        {
            y += Math.Sign(targetY - y);
            route.Add(new CoordinateDto { X = x, Y = y });
        }

        return route;
    }
}