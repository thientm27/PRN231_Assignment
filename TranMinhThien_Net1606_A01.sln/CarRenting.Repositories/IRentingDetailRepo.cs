using CarRenting.DTOs;

namespace CarRenting.Repositories;

public interface IRentingDetailRepo : IBaseRepo<RentingDetailDto>
{
    public Task<List<int>> GetCarAlreadyRented(DateTime startDay, DateTime endDay);
    public Task<List<RentingDetailDto>?> GetsByIdAsync(int id);
}