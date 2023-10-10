using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRenting.DTOs;

namespace CarRentingOData.DTOs
{
    public class CarRentalDto
    {
        [Key]
        public int CustomerID { get; set; }

        [Key]
        public int CarID { get; set; }

        [Key]
        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public decimal RentPrice { get; set; }

        public string Status { get; set; }

        public CustomerDto? Customer { get; set; }
        public CarDto? Car { get; set; }
    }
}
