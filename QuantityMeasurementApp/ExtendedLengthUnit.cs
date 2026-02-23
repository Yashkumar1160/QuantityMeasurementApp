using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public static class ExtendedLengthUnit
    {

        // Method to get conversion factor to base unit (Feet)
        public static double GetConversionFactor(LengthUnit unit)
        {
            switch (unit)
            {
                //Feet
                case LengthUnit.Feet:
                    return 1.0;

                //Inch
                case LengthUnit.Inch:
                    return 1.0 / 12.0;
                
                //Yard
                case LengthUnit.Yard:
                    return 3.0;
                
                //Centimeter
                case LengthUnit.Centimeter:
                    return 0.0328084;

                default:
                    throw new ArgumentException("Invalid unit type");
            }
        }
        //Method to Convert any value to base unit
        public static double ConvertToBase(double value, LengthUnit unit)
        {
            return value * GetConversionFactor(unit);
        }
        //Method to Convert from base unit (Feet) to target unit
        public static double ConvertFromBase(double valueInFeet, LengthUnit target)
        {
            return valueInFeet / GetConversionFactor(target);
        }
    }
}