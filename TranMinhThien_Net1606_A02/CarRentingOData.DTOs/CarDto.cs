using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarRentingOData.DTOs;

public class CarDto
{
    [Key]
    public int CarID { get; set; }

    public string CarName { get; set; }

    public int CarModelYear { get; set; }

    public string Color { get; set; }

    public int Capacity { get; set; }

    public string Description { get; set; }

    public DateTime ImportDate { get; set; }
    public int ProducerID { get; set; }
    public decimal RentPrice { get; set; }
    public string Status { get; set; }

    // Navigation property to represent the many-to-one relationship
    public CarProducerDto? CarProducer { get; set; }
}

