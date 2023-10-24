﻿using CarRenting.DTOs;
using CarRenting.DTOs.Request;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentingController : ControllerBase
{
    private readonly IRentingRepo _repository = new RentingRepo();
    private readonly IRentingDetailRepo _rentingDetailRepo = new RentingDetailRepo();
    private readonly ICarInformationRepo _carInformationRepo = new CarInformationRepo();

    [HttpGet]
    public async Task<IActionResult> GetRenting()
    {
        var rst = await _repository.GetAsync();
        return Ok(rst);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdCustomer(int id)
    {
        var  rst= _repository.GetByIdCustomerAsync(id);
        return Ok(rst);
    }

    [HttpGet("RentingDetail/{id}")]
    public async Task<IActionResult> GetDetailsById(int id)
    {
        var rst = await _rentingDetailRepo.GetsByIdAsync(id);
        return Ok(rst);
    }

    [HttpGet("Renting/{id}")]
    public async Task<IActionResult> GetRentingById(int id)
    {
        var rst =  await _repository.GetByIdAsync(id);
        return Ok(rst);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRenting(NewRenting data)
    {
        //var result = await _repository.AddWithDetailsAsync(data);
        return NoContent();
    }

    [HttpPost("AvailableCar")]
    public async Task<IActionResult> GetAvailableCar(GetAvailableCarRequest data)
    {
        var rentedList = await _rentingDetailRepo.GetCarAlreadyRented(data.StartDateTime, data.EndDateTime);
        var listCar = await _carInformationRepo.GetCarAvailable();
        var result = listCar.Where(o => !rentedList.Contains(o.CarId)).ToList();
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}