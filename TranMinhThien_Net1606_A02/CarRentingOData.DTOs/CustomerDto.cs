using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentingOData.DTOs;

public class CustomerDto
{
    [Key]
    public int CustomerID { get; set; }
    public string CustomerName { get; set; }
    public string Mobile { get; set; }
    [DataType(DataType.Date)] public DateTime Birthday { get; set; }
    public string IdentityCard { get; set; }
    public string LicenceNumber { get; set; }
    [DataType(DataType.Date)] public DateTime LicenceDate { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}