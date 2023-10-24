using CarRenting.API.Models;
using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CarInformationController : ControllerBase
{
    private readonly ICarInformationRepo _repository = new CarInformationRepo();

    [HttpGet]
    [Authorize(Roles = UserRoles.Admin)]
    [Authorize(Roles = UserRoles.Customer)]
    public async Task<IActionResult> Get()
    {
        var reslut = await _repository.GetAsync();
        return Ok(reslut);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
         var reslut = await _repository.GetByIdAsync(id);
        return Ok(reslut);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create(CarInformation newCar)
    {
        var reslut = await _repository.AddAsync(newCar);
        return Ok(reslut);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(CarInformation newCar)
    {
        var reslut = await _repository.UpdateAsync(newCar);
        return Ok(reslut);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }

}