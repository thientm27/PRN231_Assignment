using System.ComponentModel.DataAnnotations;

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
