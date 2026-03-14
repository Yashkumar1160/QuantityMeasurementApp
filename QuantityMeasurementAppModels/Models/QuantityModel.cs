using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementAppModels.Models
{
    public class QuantityModel
    {
        //Properties
        public double Value { get; }
        public string UnitName { get; }
        public string MeasurementType { get; }

        // Constructor
        public QuantityModel(double value, string unitName, string measurementType)
        {
            Value = value;
            UnitName = unitName;
            MeasurementType = measurementType;
        }

        // Override ToString Method
        public override string ToString()
        {
            return Value + " " + UnitName;
        }
    }
}