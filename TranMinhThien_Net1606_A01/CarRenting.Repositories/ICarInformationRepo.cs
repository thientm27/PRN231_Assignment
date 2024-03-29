using CarRenting.DTOs;

namespace CarRenting.Repositories;

public interface ICarInformationRepo : IBaseRepo<CarInformationDto>
{
    public Task<List<CarInformationDto>> GetCarAvailable();
}