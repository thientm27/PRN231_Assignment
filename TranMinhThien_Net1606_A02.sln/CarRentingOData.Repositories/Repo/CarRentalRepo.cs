using AutoMapper;
using CarRentingOData.BOs;
using CarRentingOData.DTOs;
using CarRentingOData.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class CarRentalRepo : ICarRentalRepo
{
    private readonly CarRentingDbContext _context;
    private readonly IMapper _mapper;

    public CarRentalRepo()
    {
        _context = new CarRentingDbContext();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CarRental, CarRentalDto>();
            cfg.CreateMap<CarRentalDto, CarRental>()
         .ForMember(dest => dest.PickupDate, opt => opt.MapFrom(src => src.PickupDate))
         .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate));

            cfg.CreateMap<Customer, CustomerDto>();
            cfg.CreateMap<Car, CarDto>();
        });

        _mapper = new Mapper(config);
    }

    public async Task<List<CarRentalDto>> GetAsync()
    {
        var entities = await _context.CarRentals
            .Include(o => o.Customer)
            .Include(o => o.Car)
            .ToListAsync();
        return entities.Select(dto => _mapper.Map<CarRentalDto>(dto)).ToList();
    }

    public async Task<CarRentalDto?> AddAsync(CarRentalDto customerDto)
    {
        try
        {

            var existed = await _context.CarRentals.FirstOrDefaultAsync(od
            => od.CustomerID == customerDto.CustomerID
            && od.CarID == customerDto.CarID
            && od.PickupDate == customerDto.PickupDate);
            if (existed != null)
            {
                existed.Status = "Pending";
                await _context.SaveChangesAsync();

                return _mapper.Map<CarRentalDto>(existed);
            }
            else
            {
                var entity = new CarRental
                {
                    CustomerID = customerDto.CustomerID,
                    CarID = customerDto.CarID,
                    PickupDate = customerDto.PickupDate,
                    ReturnDate = customerDto.ReturnDate,
                    RentPrice = customerDto.RentPrice,
                    Status = customerDto.Status,
                };

                var rEntry = await _context.CarRentals.AddAsync(entity);
                await _context.SaveChangesAsync();
                return _mapper.Map<CarRentalDto>(rEntry.Entity);
            }


        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<CarRentalDto?> GetByAsync(int? customerID, int ?carId, DateTime ?pickDate)
    {
        var query = _context.CarRentals.Include(o => o.Customer).AsQueryable();

        if (customerID != null)
        {
            query = query.Where(od => od.CustomerID == customerID);
        }

        if (carId != null)
        {
            query = query.Where(od => od.CarID == carId);
        }

        if (pickDate != null)
        {
            query = query.Where(od => od.PickupDate == pickDate);
        }

        var entity = await query.FirstOrDefaultAsync();

        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<CarRentalDto>(entity);
    }

    public async Task<bool> DeleteAsync(CarRentalDto deleteObj)
    {
        var carInformation =  await GetByAsync(deleteObj.CustomerID, deleteObj.CarID, deleteObj.PickupDate);
        if (carInformation != null)
        {
            carInformation.Status = "0";
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteAsync(int? customerID, int? carId, DateTime? pickDate)
    {
        var carInformation = await _context.CarRentals.FirstOrDefaultAsync(od
            => od.CustomerID == customerID
            && od.CarID == carId
            && od.PickupDate == pickDate);


        if (carInformation != null)
        {
            carInformation.Status = "0";
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }


    //public async Task<List<CarRentalDto>?> GetByIdCustomerAsync(int id)
    //{
    //    var entities = await _context.CarRentals
    //        .Include(o => o.Customer)
    //        .Where(od => od.CustomerId == id && od.Status != 0).ToListAsync();
    //    if (entities == null || entities.Count == 0)
    //    {
    //        return null;
    //    }

    //    return entities.Select(dto => _mapper.Map<RentingDto>(dto)).ToList();
    //}
}