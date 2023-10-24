using CarRenting.DTOs;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManufactureController : ControllerBase
{
    private readonly IManufactureRepo _repository = new ManufactureRepo();

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resulut = await _repository.GetAsync();
        return Ok(resulut);
    }
}