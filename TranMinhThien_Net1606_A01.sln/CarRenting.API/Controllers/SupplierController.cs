using CarRenting.DTOs;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierRepo _repository = new SupplierRepo();

    [HttpGet]
    public async Task<List<SupplierDto>> Get()
    {
        return await _repository.GetAsync();
    }
}