using AutoMapper;
using CarRentingOData.BOs;
using CarRentingOData.DTOs;
using CarRentingOData.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class CarProducerRepo : ICarProducerRepo
{
    private readonly CarRentingDbContext _context;
    private readonly IMapper _mapper;


    public CarProducerRepo()
    {
        _context = new CarRentingDbContext();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CarProducerDto, CarProducer>();
            cfg.CreateMap<CarProducer, CarProducerDto>();
        });

        _mapper = new Mapper(config);
    }

    public async Task<List<CarProducerDto>> GetAsync()
    {
        var entities = await _context.CarProducers.ToListAsync();
        return entities.Select(dto => _mapper.Map<CarProducerDto>(dto)).ToList();
    }
}