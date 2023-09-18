using CarRenting.DTOs;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepo _repository = new CustomerRepo();

    [HttpGet]
    public async Task<List<CustomerDto>> Get()
    {
        return await _repository.GetAsync();
    }

    [HttpGet("{id}")]
    public async Task<CustomerDto?> GetById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<CustomerDto?> Create(CustomerDto customerDto)
    {
        var createdCustomer = await _repository.AddAsync(customerDto);
        return createdCustomer;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CustomerDto customerDto)
    {
        var result = await _repository.UpdateAsync(customerDto);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }


}