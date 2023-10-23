using AutoMapper;
using CarRentingOData.BOs;
using CarRentingOData.DTOs;
using CarRentingOData.Repositories;
using Microsoft.EntityFrameworkCore;
namespace CarRenting.Repositories.Repo;

public class CarRepo : ICarRepo
{
    private readonly CarRentingDbContext _context;
    private readonly IMapper _mapper;

    public CarRepo()
    {
        _context = new CarRentingDbContext();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CarDto, Car>();
            cfg.CreateMap<Car, CarDto>();
            cfg.CreateMap<CarProducer, CarProducerDto>();
            cfg.CreateMap<CarProducerDto, CarProducer>();
        });

        _mapper = new Mapper(config);
    }

    public async Task<List<CarDto>> GetAsync()
    {
        var entities = await _context.Cars
            .Include(o => o.CarProducer)
            .ToListAsync();
        return entities.Select(dto => _mapper.Map<CarDto>(dto)).ToList();
    }
    public async Task<List<CarDto>> GetCarAvailable()
    {
        var entities = await _context.Cars
            .Include(o => o.CarProducer)
            .Where(o => o.Status != "0")
            .ToListAsync();
        return entities.Select(dto => _mapper.Map<CarDto>(dto)).ToList();
    }
    public async Task<CarDto?> AddAsync(CarDto dataDto)
    {
        var entity = _mapper.Map<Car>(dataDto);
        // var maxId = await _context.CarInformations.MaxAsync(o => o.CarId);
        // entity.CarId = maxId + 1;
        var rEntry = await _context.Cars.AddAsync(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<CarDto>(rEntry.Entity);
    }
    public async Task<CarDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Cars
            .Include(o => o.CarProducer)
            .FirstOrDefaultAsync(od => od.CarID == id);
        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<CarDto>(entity);
    }

    public async Task<CarDto?> UpdateAsync(CarDto dataDto)
    {
        var entity = _context.Cars.FirstOrDefault(od => od.CarID == dataDto.CarID);
        if (entity != null)
        {
            _context.Entry(entity).CurrentValues.SetValues(_mapper.Map<Car>(dataDto));
            await _context.SaveChangesAsync();
            return _mapper.Map<CarDto>(entity);
        }

        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var carInformation = await _context.Cars.FirstOrDefaultAsync(od => od.CarID == id);
        if (carInformation != null)
        {

            carInformation.Status = "0";
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}