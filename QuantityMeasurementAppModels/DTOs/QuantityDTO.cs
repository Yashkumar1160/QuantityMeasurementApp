using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementAppModels.DTOs
{
    public class QuantityDTO
    {

        // Encapsulated fields
        public readonly double Value;
        public readonly string UnitName;
        public readonly string MeasurementType;


        // Constructor
        public QuantityDTO(double value, string unitName, string measurementType)
        {
            Value = value;
            UnitName = unitName;
            MeasurementType = measurementType;
        }

        // Override ToString method
        public override string ToString()
        {
            return $"{Value} {UnitName}";
        }
    }
}