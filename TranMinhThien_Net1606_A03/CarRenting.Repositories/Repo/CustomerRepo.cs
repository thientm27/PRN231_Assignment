using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarRenting.Repositories.Repo
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly FUCarRentingManagementContext _context;

        public CustomerRepo()
        {
            _context = new FUCarRentingManagementContext();
          
        }

        public async Task<Customer?> LoginAsync(string email, string password)
        {
            var adminAccount = GetAdmin();
            if (email.ToLower() == adminAccount.Email.ToLower() && password == adminAccount.Password)
            {
                return new Customer()
                {
                    CustomerId = -1,
                    Email = email,
                    Password = password,
                    CustomerName = "Admin",
                };
            }

            // User check
            var loginCustomer = await _context.Customers.FirstOrDefaultAsync(od =>
                od.Email.ToLower().Equals(email.ToLower()) && od.Password.Equals(password)
                && od.CustomerStatus != 0);// not deleted account
            if (loginCustomer == null)
            {
                return null;
            }

            return loginCustomer;
        }

        private AdminAccountInfo GetAdmin()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var adminSection = config.GetSection("AdminAccount");
            var adminAccount = new AdminAccountInfo
            {
                Email = adminSection["Email"],
                Password = adminSection["Password"]
            };

            return adminAccount;
        }


        public async Task<Customer?> AddAsync(Customer customerDto)
        {
            var existedEmail =
                _context.Customers.FirstOrDefault(od =>
                    od.Email.ToLower().Equals(customerDto.Email.ToLower())
                    && od.CustomerStatus != 0);
            if (existedEmail != null)
            {
                return null;
            }
            var rEntry = await _context.Customers.AddAsync(customerDto);
            await _context.SaveChangesAsync();
            return rEntry.Entity;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(od => od.CustomerId == id);
            if (customer == null)
            {
                return null;
            }

            return customer;
        }

        public async Task<Customer?> UpdateAsync(Customer customerDto)
        {
            var existedEmail =
                _context.Customers.FirstOrDefault(od =>
                    od.Email.ToLower().Equals(customerDto.Email.ToLower()) 
                    && od.CustomerId != customerDto.CustomerId
                    && od.CustomerStatus != 0);
            if (existedEmail != null)
            {
                return null;
            }

            var customer = _context.Customers.FirstOrDefault(od => od.CustomerId == customerDto.CustomerId);
            if (customer != null)
            {
                _context.Entry(customer).CurrentValues.SetValues(customerDto);
                await _context.SaveChangesAsync();
                return customerDto;
            }

            return null;
        }

        public async Task<bool> CheckForDuplicateEmail(int id, String email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(od => od.Email == email && od.CustomerId != id);
            return customer == null;
        }

        public async Task<List<Customer>> GetAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers;
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(od => od.CustomerId == customerId);
            if (existingCustomer != null)
            {
                existingCustomer.CustomerStatus = 0;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }

    public class AdminAccountInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}