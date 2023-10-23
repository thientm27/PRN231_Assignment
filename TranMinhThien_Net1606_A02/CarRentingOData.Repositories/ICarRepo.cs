using CarRentingOData.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingOData.Repositories
{
    public interface ICarRepo
    {
        public Task<List<CarDto>> GetAsync();
        public Task<List<CarDto>> GetCarAvailable();
        public Task<CarDto?> AddAsync(CarDto dataDto);
        public Task<CarDto?> GetByIdAsync(int id);
        public Task<CarDto?> UpdateAsync(CarDto dataDto);
        public Task<bool> DeleteAsync(int id);
    }
}
