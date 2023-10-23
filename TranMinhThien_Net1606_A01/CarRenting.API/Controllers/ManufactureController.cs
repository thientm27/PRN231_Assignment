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
    public async Task<List<ManufacturerDto>> Get()
    {
        return await _repository.GetAsync();
    }
}