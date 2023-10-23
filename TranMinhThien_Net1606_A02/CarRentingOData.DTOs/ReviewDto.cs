using System.ComponentModel.DataAnnotations;

namespace CarRentingOData.DTOs
{
    public class ReviewDto
    {
        [Key]
        public int CustomerID { get; set; }

        [Key]
        public int CarID { get; set; }

        [Range(0, 5, ErrorMessage = "ReviewStar must be between 0 and 5.")]
        public int ReviewStar { get; set; }

        public string Comment { get; set; }

        public CustomerDto Customer { get; set; }
        public CarDto Car { get; set; }
    }
}
