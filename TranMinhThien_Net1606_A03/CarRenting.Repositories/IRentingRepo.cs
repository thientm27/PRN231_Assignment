

namespace CarRenting.Repositories;

public interface IRentingRepo : IBaseRepo<Renting>
{
    public Task<Renting?> AddWithDetailsAsync(Renting data);
    public Task<List<Renting>> GetByIdCustomerAsync(int id);
}