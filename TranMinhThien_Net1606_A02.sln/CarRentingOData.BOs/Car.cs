using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingOData.BOs
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarID { get; set; }

        [Required]
        public string CarName { get; set; }

        [Required]
        public int CarModelYear { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public int Capacity { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime ImportDate { get; set; }

        [Required]
        public int ProducerID { get; set; }

        [Required]
        public decimal RentPrice { get; set; }

        [Required]
        public string Status { get; set; }

        // Navigation property to represent the many-to-one relationship
        public CarProducer CarProducer { get; set; }
    }
}
