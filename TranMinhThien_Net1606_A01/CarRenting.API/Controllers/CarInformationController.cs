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
    public async Task<List<CarInformationDto>> Get()
    {
        return await _repository.GetAsync();
    }

    [HttpGet("{id}")]
    public async Task<CarInformationDto?> GetById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<CarInformationDto?> Create(CarInformationDto newCar)
    {
        var createdCustomer = await _repository.AddAsync(newCar);
        return createdCustomer;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CarInformationDto newCar)
    {
        
        var result = await _repository.UpdateAsync(newCar);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }

}