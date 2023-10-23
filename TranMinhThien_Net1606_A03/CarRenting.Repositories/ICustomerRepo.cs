using CarRenting.DTOs;

namespace CarRenting.Repositories;

public interface ICustomerRepo : IBaseRepo<CustomerDto>
{
    public Task<CustomerDto?> LoginAsync(string email, string password);
}