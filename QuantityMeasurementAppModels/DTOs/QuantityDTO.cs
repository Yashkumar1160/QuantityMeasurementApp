using System.ComponentModel.DataAnnotations;
using QuantityMeasurementAppModels.Validation;

namespace QuantityMeasurementAppModels.DTOs
{
    [ValidUnit]
    public class QuantityDTO
    {
        // Properties with attributes
        [Required(ErrorMessage = "Value is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double Value { get; set; }

        [Required(ErrorMessage = "Unit name is required")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Unit name must contain only letters")]
        public string UnitName { get; set; }

        [Required(ErrorMessage = "Measurement type is required")]
        [RegularExpression("^(Length|Weight|Volume|Temperature)$",
            ErrorMessage = "Measurement type must be Length, Weight, Volume or Temperature")]
        public string MeasurementType { get; set; }

        // Constructor
        public QuantityDTO(double value, string unitName, string measurementType)
        {
            Value = value;
            UnitName = unitName;
            MeasurementType = measurementType;
        }

        // Empty Constructor
        public QuantityDTO()
        {
        }

        // Override To String method
        public override string ToString()
        {
            return Value + " " + UnitName;
        }
    }
}