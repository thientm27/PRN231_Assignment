using CarRenting.DTOs;
using CarRenting.DTOs.Request;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using CarRentingOData.DTOs;
using CarRentingOData.DTOs.Request;
using CarRentingOData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CarRenting.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarRentalController : ControllerBase
{
    private readonly ICarRentalRepo _repository = new CarRentalRepo();

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        var result = await _repository.GetAsync();
        return Ok(result);
    }

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetByIdCustomer(int id)
    //{
    //    var result = await _repository.GetByAsync(id);
    //    return Ok(result);
    //}

    [HttpPost]
    public async Task<IActionResult> CreateRenting(NewRenting data)
    {
        foreach (var item in data.Values)
        {
            var result = await _repository.AddAsync(item);
        }
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Delete(CarRentalDto carRentalDto)
    {
        await _repository.DeleteAsync(carRentalDto.CustomerID, carRentalDto.CarID, carRentalDto.PickupDate);
        return Ok();
    }

}