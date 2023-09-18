using AutoMapper;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class ManufactureRepo : IManufactureRepo
{
    private readonly FUCarRentingManagementContext _context;
    private readonly IMapper _mapper;

    public ManufactureRepo()
    {
        _context = new FUCarRentingManagementContext();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ManufacturerDto, Manufacturer>();
            cfg.CreateMap<Manufacturer, ManufacturerDto>();
        });

        _mapper = new Mapper(config);
    }

    public async Task<List<ManufacturerDto>> GetAsync()
    {
        var entities = await _context.Manufacturers.ToListAsync();
        return entities.Select(dto => _mapper.Map<ManufacturerDto>(dto)).ToList();
    }

    public Task<ManufacturerDto?> AddAsync(ManufacturerDto customerDto)
    {
        throw new NotImplementedException();
    }

    public Task<ManufacturerDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ManufacturerDto?> UpdateAsync(ManufacturerDto customerDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int customerId)
    {
        throw new NotImplementedException();
    }
}