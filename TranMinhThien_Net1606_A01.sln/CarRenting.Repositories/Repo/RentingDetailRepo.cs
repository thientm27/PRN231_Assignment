using AutoMapper;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class RentingDetailRepo : IRentingDetailRepo
{
    private readonly FUCarRentingManagementContext _context;
    private readonly IMapper _mapper;

    public RentingDetailRepo()
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

    public async Task<List<RentingDetailDto>> GetAsync()
    {
        var entities = await _context.RentingDetails
            .Include(o => o.Car)
            .ToListAsync();
        return entities.Select(dto => _mapper.Map<RentingDetailDto>(dto)).ToList();
    }

    public async Task<RentingDetailDto?> AddAsync(RentingDetailDto dataDto)
    {
        var entity = _mapper.Map<RentingDetail>(dataDto);
        // var maxId = await _context.CarInformations.MaxAsync(o => o.CarId);
        // entity.CarId = maxId + 1;
        var rEntry = await _context.RentingDetails.AddAsync(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<RentingDetailDto>(rEntry.Entity);
    }


    public async Task<RentingDetailDto?> UpdateAsync(RentingDetailDto dataDto)
    {
        var entity = _context.RentingDetails.FirstOrDefault(od => od.CarId == dataDto.CarId);
        if (entity != null)
        {
            _context.Entry(entity).CurrentValues.SetValues(_mapper.Map<RentingDetail>(dataDto));
            await _context.SaveChangesAsync();
            return _mapper.Map<RentingDetailDto>(entity);
        }

        return null;
    }

    [Obsolete("This method cannot use")]
    public async Task<RentingDetailDto?> GetByIdAsync(int id)
    {
        var entity = await _context.RentingDetails
            .Include(o => o.Car)
            .FirstOrDefaultAsync(od => od.RentingTransactionId == id);
        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<RentingDetailDto>(entity);
    }

    [Obsolete("This method cannot use")]
    public async Task<bool> DeleteAsync(int id)
    {
        var carInformation = await _context.RentingDetails.FirstOrDefaultAsync(od => od.RentingTransactionId == id);
        if (carInformation != null)
        {
            _context.RentingDetails.Remove(carInformation);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<List<int>> GetCarAlreadyRented(DateTime startDay, DateTime endDay)
    {
        var listRented = await _context.RentingDetails.Where(o => o.StartDate >= startDay && o.EndDate <= endDay)
            .ToListAsync();
        return listRented.Select(o => o.CarId).ToList();
    }
    
}