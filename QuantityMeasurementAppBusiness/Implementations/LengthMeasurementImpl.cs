using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppModels.Enums;

namespace QuantityMeasurementAppBusiness.Implementations
{
    public class LengthMeasurementImpl : IMeasurable
    {
        private LengthUnit unit;

        // Constructor
        public LengthMeasurementImpl(LengthUnit unitType)
        {
            unit = unitType;
        }

        // Method to get conversion factor
        public double GetConversionFactor()
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return 1.0;

                case LengthUnit.Inch:
                    return 1.0 / 12.0;

                case LengthUnit.Yard:
                    return 3.0;

                case LengthUnit.Centimeter:
                    return 0.0328084;

                default:
                    throw new ArgumentException("Invalid Length Unit");
            }
        }

        // Convert value to base unit (Feet)
        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();
        }

        // Convert base unit (Feet) to target unit
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

        public string GetMeasurementType()
        {
            return "Length";
        }

        public IMeasurable GetUnitByName(string unitName)
        {
            LengthUnit parsedUnit = (LengthUnit)Enum.Parse(typeof(LengthUnit), unitName, true);
            return new LengthMeasurementImpl(parsedUnit);
        }
    }
}