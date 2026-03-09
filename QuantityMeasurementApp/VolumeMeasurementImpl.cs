using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class VolumeMeasurementImpl : IMeasurable
    {
        private VolumeUnit unit;

        //Constructor
        public VolumeMeasurementImpl(VolumeUnit unitType)
        {
            unit = unitType;
        }

        //Method to get conversion factor
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

        //Convert value to base unit (Litre)
        public double ConvertToBaseUnit(double value)
        {
            return value * GetConversionFactor();
        }

        //Convert base unit to target unit
        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / GetConversionFactor();
        }

        //Return unit name
        public string GetUnitName()
        {
            return unit.ToString();
        }
    }
}