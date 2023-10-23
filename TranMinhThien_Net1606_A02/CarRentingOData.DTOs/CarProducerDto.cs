using System;
using System.ComponentModel.DataAnnotations;
using CarRenting.DTOs;

namespace CarRentingOData.DTOs
{
    public class CarProducerDto
    {
        [Key]
        public int ProducerID { get; set; }

        public string ProducerName { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

    }
}
