using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingOData.BOs
{

    public class CarRentingDbContext : DbContext
    {
        public CarRentingDbContext()
        {

        }
        public CarRentingDbContext(DbContextOptions<CarRentingDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(GetConnectionString());
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config["ConnectionString"];
            return strConn;
        }

        // DbSet for each model
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarProducer> CarProducers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CarRental> CarRentals { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite key for CarRental
            modelBuilder.Entity<CarRental>().HasKey(cr => new { cr.CustomerID, cr.CarID, cr.PickupDate });

            // Configure relationships
            modelBuilder.Entity<Review>().HasKey(review => new { review.CustomerID, review.CarID });
            modelBuilder.Entity<Car>().HasOne(c => c.CarProducer).WithMany(cp => cp.Cars).HasForeignKey(c => c.ProducerID);
            modelBuilder.Entity<CarRental>().HasOne(cr => cr.Customer).WithMany(c => c.CarRentals).HasForeignKey(cr => cr.CustomerID);
            modelBuilder.Entity<CarRental>().HasOne(cr => cr.Car).WithMany().HasForeignKey(cr => cr.CarID);
            // Seed some fake data
            modelBuilder.Entity<CarProducer>().HasData(
                new CarProducer { ProducerID = 1, ProducerName = "Producer1", Address = "Address1", Country = "Country1" },
                new CarProducer { ProducerID = 2, ProducerName = "Producer2", Address = "Address2", Country = "Country2" }
                // Add more CarProducers as needed...
            );

            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    CarID = 1,
                    CarName = "Car1",
                    CarModelYear = 2022,
                    Color = "Red",
                    Capacity = 4,
                    Description = "Description for Car1",
                    ImportDate = DateTime.Now,
                    ProducerID = 1,
                    RentPrice = 50,
                    Status = "Available"
                },
                new Car
                {
                    CarID = 2,
                    CarName = "Car2",
                    CarModelYear = 2023,
                    Color = "Blue",
                    Capacity = 5,
                    Description = "Description for Car2",
                    ImportDate = DateTime.Now,
                    ProducerID = 2,
                    RentPrice = 60,
                    Status = "Available"
                }
                // Add more Cars as needed...
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerID = 1,
                    CustomerName = "John Doe",
                    Mobile = "1234567890",
                    Birthday = new DateTime(1985, 5, 15),
                    IdentityCard = "123456789",
                    LicenceNumber = "ABC123",
                    LicenceDate = new DateTime(2005, 7, 20),
                    Email = "john.doe@example.com",
                    Password = "1"
                },
                new Customer
                {
                    CustomerID = 2,
                    CustomerName = "Jane Smith",
                    Mobile = "9876543210",
                    Birthday = new DateTime(1990, 8, 25),
                    IdentityCard = "987654321",
                    LicenceNumber = "XYZ789",
                    LicenceDate = new DateTime(2008, 10, 12),
                    Email = "jane.smith@example.com",
                    Password = "1"
                }
                // Add more Customers as needed...
            );
            modelBuilder.Entity<CarRental>().HasData(
                new CarRental
                {
                    CustomerID = 1,
                    CarID = 1,
                    PickupDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(3),
                    RentPrice = 100,
                    Status = "Completed"
                },
                new CarRental
                {
                    CustomerID = 2,
                    CarID = 2,
                    PickupDate = DateTime.Now.AddDays(5),
                    ReturnDate = DateTime.Now.AddDays(10),
                    RentPrice = 120,
                    Status = "Pending"
                }
                // Add more CarRental data as needed...
            );

            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    CustomerID = 1,
                    CarID = 1,
                    ReviewStar = 4,
                    Comment = "Good experience with this car."
                },
                new Review
                {
                    CustomerID = 2,
                    CarID = 2,
                    ReviewStar = 5,
                    Comment = "Excellent car! Highly recommended."
                }
                // Add more Reviews as needed...
            );


            base.OnModelCreating(modelBuilder);
        }
    }
}
