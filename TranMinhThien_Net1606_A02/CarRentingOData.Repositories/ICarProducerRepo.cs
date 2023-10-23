using CarRentingOData.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingOData.Repositories
{
    public interface ICarProducerRepo
    {
        public Task<List<CarProducerDto>> GetAsync();
    }
}
