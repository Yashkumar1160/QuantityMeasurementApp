using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace QuantityMeasurementApp
{
    public class LengthMeasurementImpl : IMeasurable
    {
        private LengthUnit unit;

        public LengthMeasurementImpl(LengthUnit unitType)
        {
            unit = unitType;
        }

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

        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();
        }

        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / GetConversionFactor();
        }

        public string GetUnitName()
        {
            return unit.ToString();
        }
    }
}