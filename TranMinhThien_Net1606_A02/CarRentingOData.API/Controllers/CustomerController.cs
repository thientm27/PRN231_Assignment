using CarRenting.DTOs.Request;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using CarRentingOData.DTOs;

namespace CarRenting.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ODataController
{
    private readonly ICustomerRepo _repository = new CustomerRepo();

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        var result = await _repository.GetAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [EnableQuery]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    [EnableQuery]
    public async Task<IActionResult> Create(CustomerDto customerDto)
    {
        var createdCustomer = await _repository.AddAsync(customerDto);
        return Ok(createdCustomer);
    }

    [HttpPut]
    [EnableQuery]
    public async Task<IActionResult> Update([FromBody] CustomerDto customerDto)
    {
        var result = await _repository.UpdateAsync(customerDto);
        return Ok(result);
    }


    [HttpDelete("{id}")]
    [EnableQuery]
    public async Task<IActionResult> Delete( int id)
    {

        var customer = await _repository.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return Ok();
    }

    [HttpPost("login")]
    [EnableQuery]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var loginCustomer = await _repository.LoginAsync(loginRequest.Email, loginRequest.Password);

        if (loginCustomer == null)
        {
            return NotFound("Invalid email or password");
        }

        return Ok(loginCustomer);
    }
}