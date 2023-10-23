using CarRenting.Repositories.Repo;
using CarRentingOData.DTOs;
using CarRentingOData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CarRenting.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly ICarRepo _repository = new CarRepo();

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        var result =  await _repository.GetAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult>  Create(CarDto newCar)
    {
        var result = await _repository.AddAsync(newCar);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CarDto newCar)
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