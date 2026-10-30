using DriverDispatch.Api.Contracts;
using DriverDispatch.Api.Options;
using DriverDispatch.Api.Services;
using DriverDispatch.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DriverDispatch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DispatchController : ControllerBase
{
    private readonly DriverStore _store;
    private readonly IDispatchService _dispatchService;
    private readonly MapSettings _map;

    public DispatchController(
        DriverStore store,
        IDispatchService dispatchService,
        IOptions<MapSettings> map)
    {
        _store = store;
        _dispatchService = dispatchService;
        _map = map.Value;
    }

    [HttpPost("driver/coordinates")]
    public IActionResult SetDriverCoordinates([FromBody] DriverCoordinatesRequest request)
    {
        if (request.X < 0 || request.X >= _map.N || request.Y < 0 || request.Y >= _map.M)
        {
            // Если водитель существует — удаляем старые координаты (по заданию)
            if (_store.TryGet(request.Id, out _))
                _store.Remove(request.Id);

            return BadRequest(new { message = "Координаты некорректны" });
        }

        bool driverExists = _store.TryGet(request.Id, out _);

        if (driverExists && _store.IsCoordinateOccupiedByOther(request.Id, request.X, request.Y))
        {
            return BadRequest(new { message = "Здесь уже находится другой водитель" });
        }

        if (!driverExists && _store.IsCoordinateOccupied(request.X, request.Y))
        {
            return BadRequest(new { message = "Здесь уже находится другой водитель" });
        }

        _store.Set(new Driver(request.Id, request.X, request.Y));

        var message = driverExists
            ? "Координаты успешно изменены"
            : "Координаты успешно добавлены";

        return Ok(new { message });
    }

    [HttpPost("order/assign")]
    public async Task<IActionResult> AssignDriver([FromBody] OrderRequest request)
    {
        if (request.X < 0 || request.X >= _map.N || request.Y < 0 || request.Y >= _map.M)
            return BadRequest(new { message = "Координаты некорректны" });

        var result = await _dispatchService.AssignDriverAsync(request.X, request.Y);

        if (result == null)
            return BadRequest(new { message = "Свободных водителей нет" });

        return Ok(result);
    }
}