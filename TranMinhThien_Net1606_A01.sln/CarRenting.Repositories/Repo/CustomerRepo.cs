using AutoMapper;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo
{
    public class CustomerRepo : ICustomerRepo
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

        public async Task<CustomerDto?> AddAsync(CustomerDto customerDto)
        {
            var existedEmail =
                _context.Customers.FirstOrDefault(od => od.Email.ToLower().Equals(customerDto.Email.ToLower()));
            if (existedEmail != null)
            {
                return null;
            }

            var customer = _mapper.Map<Customer>(customerDto);
            var rEntry = await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return _mapper.Map<CustomerDto>(rEntry.Entity);
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
            var existedEmail =
                _context.Customers.FirstOrDefault(od =>
                    od.Email.ToLower().Equals(customerDto.Email.ToLower()) && od.CustomerId != customerDto.CustomerId);
            if (existedEmail != null)
            {
                return null;
            }

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
            return customers.Select(dto => _mapper.Map<CustomerDto>(dto)).ToList();
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