using AutoMapper;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.DTOs.Request;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class RentingRepo : IRentingRepo
{
    private readonly FUCarRentingManagementContext _context;
    private readonly IMapper _mapper;

    public RentingRepo()
    {
        _context = new FUCarRentingManagementContext();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RentingDto, RentingTransaction>();
            cfg.CreateMap<RentingTransaction, RentingDto>();
            cfg.CreateMap<RentingDetailDto, RentingDetail>();
            cfg.CreateMap<RentingDetail, RentingDetailDto>();
        });

        _mapper = new Mapper(config);
    }

    public async Task<List<RentingDto>> GetAsync()
    {
        var entities = await _context.RentingTransactions
            .Include(o => o.Customer)
            .ToListAsync();
        return entities.Select(dto => _mapper.Map<RentingDto>(dto)).ToList();
    }

    public async Task<RentingDto?> AddAsync(RentingDto customerDto)
    {
        var entity = _mapper.Map<RentingTransaction>(customerDto);
        // var maxId = await _context.CarInformations.MaxAsync(o => o.CarId);
        // entity.CarId = maxId + 1;
        var rEntry = await _context.RentingTransactions.AddAsync(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<RentingDto>(rEntry.Entity);
    }

    public async Task<RentingDto?> GetByIdAsync(int id)
    {
        var entity = await _context.RentingTransactions
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(od => od.RentingTransationId == id);
        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<RentingDto>(entity);
    }

    public async Task<RentingDto?> UpdateAsync(RentingDto dataDto)
    {
        var entity =
            _context.RentingTransactions.FirstOrDefault(od => od.RentingTransationId == dataDto.RentingTransationId);
        if (entity != null)
        {
            _context.Entry(entity).CurrentValues.SetValues(_mapper.Map<RentingDto>(dataDto));
            await _context.SaveChangesAsync();
            return _mapper.Map<RentingDto>(entity);
        }

        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var carInformation = await _context.RentingTransactions.FirstOrDefaultAsync(od => od.RentingTransationId == id);
        if (carInformation != null)
        {
            _context.RentingTransactions.Remove(carInformation);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<RentingDto?> AddWithDetailsAsync(NewRenting data)
    {
        RentingDetailRepo rentingDetailRepo = new RentingDetailRepo();
        var renting = await AddAsync(data.rentingDto);
        if (renting == null)
        {
            return null;
        }

        foreach (var detail in data.rentingDetails)
        {
            detail.RentingTransactionId = renting.RentingTransationId;
            await rentingDetailRepo.AddAsync(detail);
        }

        return renting;
    }

    public async Task<List<RentingDto>?> GetByIdCustomerAsync(int id)
    {
        var entities = await _context.RentingTransactions
            .Include(o => o.Customer)
            .Where(od => od.CustomerId == id).ToListAsync();
        if (entities == null || entities.Count == 0)
        {
            return null;
        }
        return entities.Select(dto => _mapper.Map<RentingDto>(dto)).ToList();
    }
}