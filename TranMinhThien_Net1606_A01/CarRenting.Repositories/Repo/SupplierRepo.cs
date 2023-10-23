using AutoMapper;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class SupplierRepo : ISupplierRepo
{
    private readonly FUCarRentingManagementContext _context;
    private readonly IMapper _mapper;

    public SupplierRepo()
    {
        _context = new FUCarRentingManagementContext();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SupplierDto, Supplier>();
            cfg.CreateMap<Supplier, SupplierDto>();
        });

        _mapper = new Mapper(config);
    }

    public async Task<List<SupplierDto>> GetAsync()
    {
        var entities = await _context.Suppliers.ToListAsync();
        return entities.Select(dto => _mapper.Map<SupplierDto>(dto)).ToList();
    }

    public Task<SupplierDto?> AddAsync(SupplierDto customerDto)
    {
        throw new NotImplementedException();
    }

    public Task<SupplierDto?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<SupplierDto?> UpdateAsync(SupplierDto customerDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int customerId)
    {
        throw new NotImplementedException();
    }
}