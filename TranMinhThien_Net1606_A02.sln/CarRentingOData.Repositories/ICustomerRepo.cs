
namespace CarRenting.Repositories;

public interface ICustomerRepo 
{
    public Task<CustomerDto?> LoginAsync(string email, string password);
}