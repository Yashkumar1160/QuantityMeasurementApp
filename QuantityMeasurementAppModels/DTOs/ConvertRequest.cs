using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementAppModels.DTOs
{
    public class ConvertRequest
    {
        [Required(ErrorMessage = "ThisQuantityDTO is required")]
        public QuantityDTO ThisQuantityDTO { get; set; }

        [Required(ErrorMessage = "TargetUnit is required")]
        public string TargetUnit { get; set; }
    }
}