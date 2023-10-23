using CarRenting.DTOs;
using CarRenting.DTOs.Request;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentingController : ControllerBase
{
    private readonly IRentingRepo _repository = new RentingRepo();
    private readonly IRentingDetailRepo _rentingDetailRepo = new RentingDetailRepo();
    private readonly ICarInformationRepo _carInformationRepo = new CarInformationRepo();

    [HttpGet]
    public async Task<List<RentingDto>> GetRenting()
    {
        return await _repository.GetAsync();
    }

    [HttpGet("{id}")]
    public async Task<List<RentingDto>> GetByIdCustomer(int id)
    {
        return await _repository.GetByIdCustomerAsync(id);
    }

    [HttpGet("RentingDetail/{id}")]
    public async Task<List<RentingDetailDto>?> GetDetailsById(int id)
    {
        return await _rentingDetailRepo.GetsByIdAsync(id);
    }

    [HttpGet("Renting/{id}")]
    public async Task<RentingDto?> GetRentingById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<RentingDto?> CreateRenting(NewRenting data)
    {
        var result = await _repository.AddWithDetailsAsync(data);
        return result;
    }

    [HttpPost("AvailableCar")]
    public async Task<List<CarInformationDto>> GetAvailableCar(GetAvailableCarRequest data)
    {
        var rentedList = await _rentingDetailRepo.GetCarAlreadyRented(data.StartDateTime, data.EndDateTime);
        var listCar = await _carInformationRepo.GetCarAvailable();
        var result = listCar.Where(o => !rentedList.Contains(o.CarId)).ToList();
        return result;
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}