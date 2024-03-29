﻿using CarRentingOData.DTOs;

namespace CarRentingOData.Repositories
{
    public interface ICarRentalRepo
    {
        public  Task<List<CarRentalDto>> GetAsync();
        public  Task<CarRentalDto?> AddAsync(CarRentalDto customerDto);
        public  Task<CarRentalDto?> GetByAsync(int? customerID, int? carId, DateTime? pickDate);
        public  Task<bool> DeleteAsync(int? customerID, int? carId, DateTime? pickDate);
        public  Task<bool> DeleteAsync(CarRentalDto deleteObj);


    }
}
