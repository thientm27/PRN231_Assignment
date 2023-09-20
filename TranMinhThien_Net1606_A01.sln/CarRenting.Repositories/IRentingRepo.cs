using CarRenting.DTOs;
using CarRenting.DTOs.Request;

namespace CarRenting.Repositories;

public interface IRentingRepo : IBaseRepo<RentingDto>
{
    public Task<RentingDto?> AddWithDetailsAsync(NewRenting data);
    public Task<List<RentingDto>> GetByIdCustomerAsync(int id);
}