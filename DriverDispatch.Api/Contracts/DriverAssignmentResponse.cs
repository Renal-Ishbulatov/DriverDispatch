namespace DriverDispatch.Api.Contracts;

public class DriverAssignmentResponse
{
    public int DriverId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int RouteLength { get; set; }
    public List<CoordinateDto> Route { get; set; } = new();
}

public class CoordinateDto
{
    public int X { get; set; }
    public int Y { get; set; }
}