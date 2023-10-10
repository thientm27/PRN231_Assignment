using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarRentingOData.BOs
{
    public class CarRental
    {
        [Key]
        [Column(Order = 0)]
        public int CustomerID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int CarID { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime PickupDate { get; set; }

        [Required]
        [GreaterThan(nameof(PickupDate))] 
        public DateTime ReturnDate { get; set; }

        [Required]
        public decimal RentPrice { get; set; }

        [Required]
        public string Status { get; set; }

        // Navigation properties to represent the many-to-one relationships
        public Customer? Customer { get; set; }
        public Car? Car { get; set; }
    }
}
public class GreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public GreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var propertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);

        if (propertyInfo == null)
            return new ValidationResult($"Unknown property: {_comparisonProperty}");

        var comparisonValue = propertyInfo.GetValue(validationContext.ObjectInstance) as IComparable;

        if (comparisonValue == null)
            return new ValidationResult($"The property {_comparisonProperty} is not IComparable");

        if (value == null || (value as IComparable).CompareTo(comparisonValue) <= 0)
            return new ValidationResult($"{validationContext.DisplayName} must be greater than {_comparisonProperty}");

        return ValidationResult.Success;
    }
}