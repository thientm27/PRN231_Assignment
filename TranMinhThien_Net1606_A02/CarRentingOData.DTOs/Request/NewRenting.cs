using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingOData.DTOs.Request
{
    public class NewRenting
    {
       public List<CarRentalDto> Values { get; set; }

        public NewRenting() { 
            Values = new List<CarRentalDto>();
        }
    }
}
