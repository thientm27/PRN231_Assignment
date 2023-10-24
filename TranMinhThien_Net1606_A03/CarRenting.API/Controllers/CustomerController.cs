
using CarRenting.BusinessObjects.Models;
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
    public async Task<IActionResult> Create(Customer customerDto)
    {
        var createdCustomer = await _repository.AddAsync(customerDto);
        return Ok(createdCustomer);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Customer customerDto)
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


