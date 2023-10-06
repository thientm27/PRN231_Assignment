using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingOData.BOs
{
    public class Review
    {
        [Key]
        [Required]
        public int CustomerID { get; set; }

        [Key]
        [Required]
        public int CarID { get; set; }

        [Required]
        [Range(0, 5, ErrorMessage = "ReviewStar must be between 0 and 5.")]
        public int ReviewStar { get; set; }

        [Required]
        public string Comment { get; set; }

        // Navigation properties to represent the many-to-one relationships
        public Customer Customer { get; set; }
        public Car Car { get; set; }
    }
}
