using CarRenting.DTOs;

namespace CarRenting.Repositories.Repo;

public class RentingRepo : IRentingRepo, IRentingDetailRepo
{
    Task<List<RentingDto>> IBaseRepo<RentingDto>.GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<RentingDetailDto?> AddAsync(RentingDetailDto customerDto)
    {
        throw new NotImplementedException();
    }

    Task<RentingDetailDto?> IBaseRepo<RentingDetailDto>.GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<RentingDetailDto?> UpdateAsync(RentingDetailDto customerDto)
    {
        throw new NotImplementedException();
    }

    Task<bool> IBaseRepo<RentingDetailDto>.DeleteAsync(int customerId)
    {
        throw new NotImplementedException();
    }

    public Task<RentingDto?> AddAsync(RentingDto customerDto)
    {
        throw new NotImplementedException();
    }

    Task<List<RentingDetailDto>> IBaseRepo<RentingDetailDto>.GetAsync()
    {
        throw new NotImplementedException();
    }

    Task<RentingDto?> IBaseRepo<RentingDto>.GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<RentingDto?> UpdateAsync(RentingDto customerDto)
    {
        throw new NotImplementedException();
    }

    Task<bool> IBaseRepo<RentingDto>.DeleteAsync(int customerId)
    {
        throw new NotImplementedException();
    }
}