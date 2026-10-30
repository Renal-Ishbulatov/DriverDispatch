using DriverDispatch.Api.Contracts;

namespace DriverDispatch.Api.Services;

public interface IDispatchService
{
    Task<DriverAssignmentResponse?> AssignDriverAsync(int orderX, int orderY);
}