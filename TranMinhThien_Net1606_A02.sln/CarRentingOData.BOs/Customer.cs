using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingOData.BOs
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public string IdentityCard { get; set; }

        [Required]
        public string LicenceNumber { get; set; }

        [Required]
        public DateTime LicenceDate { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // Navigation property to represent the one-to-many relationship
        public List<CarRental> CarRentals { get; set; }
    }
}
