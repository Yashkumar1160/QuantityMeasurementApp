using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class WeightMeasurementImpl : IMeasurable
    {
        private WeightUnit unit;

        public WeightMeasurementImpl(WeightUnit unitType)
        {
            unit = unitType;
        }

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