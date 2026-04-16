using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppModels.Enums;

namespace QuantityMeasurementAppBusiness.Implementations
{
    public class WeightMeasurementImpl : IMeasurable
    {
        private WeightUnit unit;

        // Constructor
        public WeightMeasurementImpl(WeightUnit unitType)
        {
            unit = unitType;
        }

        // Method to get conversion factor
        public double GetConversionFactor()
        {
            switch (unit)
            {
                case WeightUnit.Kilogram:
                    return 1.0;

                case WeightUnit.Gram:
                    return 0.001;

                case WeightUnit.Pound:
                    return 0.453592;

                default:
                    throw new ArgumentException("Invalid Weight Unit");
            }
        }

        // Convert value to base unit (Kilogram)
        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();
        }

        // Convert base unit to target unit
        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / GetConversionFactor();
        }

        // Method to get unit name
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
            return "Weight";
        }

        // Method to get unit by name
        public IMeasurable GetUnitByName(string unitName)
        {
            WeightUnit parsedUnit = (WeightUnit)Enum.Parse(typeof(WeightUnit), unitName, true);
            return new WeightMeasurementImpl(parsedUnit);
        }
    }
}