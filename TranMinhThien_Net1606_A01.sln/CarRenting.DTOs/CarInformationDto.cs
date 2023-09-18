namespace CarRenting.DTOs;

public class CarInformationDto
{
    public int CarId { get; set; }
    public string CarName { get; set; } = null!;
    public string? CarDescription { get; set; }
    public int? NumberOfDoors { get; set; }
    public int? SeatingCapacity { get; set; }
    public string? FuelType { get; set; }
    public int? Year { get; set; }
    public int ManufacturerId { get; set; }
    public int SupplierId { get; set; }
    public byte? CarStatus { get; set; }
    public decimal? CarRentingPricePerDay { get; set; }

    public virtual ManufacturerDto Manufacturer { get; set; } = null!;
    public virtual SupplierDto Supplier { get; set; } = null!;
}

public class ManufacturerDto
{
    public int ManufacturerId { get; set; }
    public string ManufacturerName { get; set; } = null!;
}

public class SupplierDto
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = null!;
}