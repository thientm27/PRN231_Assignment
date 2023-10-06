using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarRentingOData.BOs
{
    public class CarProducer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProducerID { get; set; }

        [Required]
        public string ProducerName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Country { get; set; }

        // Navigation property to represent the one-to-many relationship
        public List<Car> Cars { get; set; }
    }

}
