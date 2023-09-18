using AutoMapper;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo
{
    public class CustomerRepo : IBaseRepo<CustomerDto>
    {
        private readonly FUCarRentingManagementContext _context;
        private readonly IMapper _mapper;

        public CustomerRepo()
        {
            _context = new FUCarRentingManagementContext();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerDto, Customer>();
                cfg.CreateMap<Customer, CustomerDto>();
                cfg.CreateMap<List<CustomerDto>, List<Customer>>();
            });

            _mapper = new Mapper(config);
        }

        public async Task<CustomerDto?> LoginAsync(string email, string password)
        {
            var loginCustomer = await _context.Customers.FirstOrDefaultAsync(od =>
                od.Email.ToLower().Equals(email.ToLower()) && od.Password.Equals(password));
            if (loginCustomer == null)
            {
                return null;
            }

            return _mapper.Map<CustomerDto>(loginCustomer);
        }

        public async Task<CustomerDto> AddAsync(CustomerDto customerDto)
        {
            Customer customer = _mapper.Map<Customer>(customerDto);
            var maxId = await _context.Customers.MaxAsync(o => o.CustomerId);
            customer.CustomerId = maxId + 1;
            var rEntry = await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return _mapper.Map<CustomerDto>(rEntry);
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(od => od.CustomerId == id);
            if (customer == null)
            {
                return null;
            }

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto?> UpdateAsync(CustomerDto customerDto)
        {
            var customer = _context.Customers.FirstOrDefault(od => od.CustomerId == customerDto.CustomerId);
            if (customer != null)
            {
                _context.Entry(customer).CurrentValues.SetValues(_mapper.Map<Customer>(customerDto));
                await _context.SaveChangesAsync();
                return _mapper.Map<CustomerDto>(customer);
            }
            return null;
        }

        public async Task<bool> CheckForDuplicateEmail(int id, String email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(od => od.Email == email && od.CustomerId != id);
            return customer == null;
        }

        public async Task<List<CustomerDto>> GetAsync()
        {
            var customers = await _context.Customers.Where(od => od.CustomerStatus == 1).ToListAsync();
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(od => od.CustomerId == customerId);
            if (existingCustomer != null)
            {
                _context.Customers.Remove(existingCustomer);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}