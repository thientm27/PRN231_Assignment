using CarRentingOData.DTOs;

namespace CarRenting.Repositories;

public interface ICustomerRepo 
{
    public Task<CustomerDto?> LoginAsync(string email, string password);
    public  Task<CustomerDto?> AddAsync(CustomerDto customerDto);
    public Task<CustomerDto?> GetByIdAsync(int id);
    public Task<CustomerDto?> UpdateAsync(CustomerDto customerDto);
    public Task<List<CustomerDto>> GetAsync();
    public Task<bool> DeleteAsync(int customerId);
}