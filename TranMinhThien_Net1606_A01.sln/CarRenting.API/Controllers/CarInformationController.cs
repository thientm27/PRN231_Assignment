using CarRenting.DTOs;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CarInformationController : ControllerBase
{
    private readonly ICarInformationRepo _repository = new CarInformationRepo();

    [HttpGet]
    public async Task<List<CarInformationDto>> GetCustomers()
    {
        return await _repository.GetAsync();
    }

    [HttpGet("{id}")]
    public async Task<CarInformationDto?> GetCustomersById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<CarInformationDto?> CreateCustomer(CarInformationDto newCar)
    {
        var createdCustomer = await _repository.AddAsync(newCar);
        return createdCustomer;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, CarInformationDto newCar)
    {
        
        var result = await _repository.UpdateAsync(newCar);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }

}