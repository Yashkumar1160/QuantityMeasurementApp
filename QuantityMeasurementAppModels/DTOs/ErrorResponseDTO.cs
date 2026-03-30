using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementAppModels.DTOs
{
    public class ErrorResponseDTO
    {
        public string Timestamp { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
    }
}