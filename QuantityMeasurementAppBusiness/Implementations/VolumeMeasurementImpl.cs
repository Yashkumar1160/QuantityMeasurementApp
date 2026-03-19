using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppModels.Enums;

namespace QuantityMeasurementAppBusiness.Implementations
{
    public class VolumeMeasurementImpl : IMeasurable
    {
        private VolumeUnit unit;

        // Constructor
        public VolumeMeasurementImpl(VolumeUnit unitType)
        {
            unit = unitType;
        }

        // Method to get conversion factor
        public double GetConversionFactor()
        {
            switch (unit)
            {
                case VolumeUnit.Litre:
                    return 1.0;

                case VolumeUnit.Millilitre:
                    return 0.001;

                case VolumeUnit.Gallon:
                    return 3.78541;

                default:
                    throw new ArgumentException("Invalid Volume Unit");
            }
        }

        // Convert value to base unit (Litre)
        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();
        }

        // Convert base unit to target unit
        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / GetConversionFactor();
        }

        // Return unit name
        public string GetUnitName()
        {
            return unit.ToString();
        }

        // Method to indicate arithmetic is supported
        public bool SupportsArithmetic()
        {
            return true;
        }

        // Validation method (no restriction)
        public void ValidateOperationSupport(string operation)
        {
            // All arithmetic operations supported
        }

        // Method to get measurement type
        public string GetMeasurementType()
        {
            return "Volume";
        }

        // Method to get unit by name
        public IMeasurable GetUnitByName(string unitName)
        {
            VolumeUnit parsedUnit = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), unitName, true);
            return new VolumeMeasurementImpl(parsedUnit);
        }
    }
}