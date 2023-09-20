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

    [HttpPost]
    public async Task<RentingDto?> CreateRenting(NewRenting data)
    {
        var result = await _repository.AddWithDetailsAsync(data);
        return result;
    }

    [HttpPost]
    public async Task<List<CarInformationDto>> GetAvailableCar(GetAvailableCarRequest data)
    {
        var rentedList = await _rentingDetailRepo.GetCarAlreadyRented(data.StartDateTime, data.EndDateTime);
        var listCar = await _carInformationRepo.GetAsync();
        var result = listCar.Where(o => !rentedList.Contains(o.CarId)).ToList();
        return result;
    }
}