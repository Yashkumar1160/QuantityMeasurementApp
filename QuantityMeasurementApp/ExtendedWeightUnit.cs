using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public static class ExtendedWeightUnit
    {
        //Method to GetConversionFactor related to kilogram
        public static double GetConversionFactor(WeightUnit unit)
        {
            switch (unit)
            {
                //Kilogram
                case WeightUnit.Kilogram:
                return 1.0;

                //Gram
                case WeightUnit.Gram:
                return 0.001;

                //Pound
                case WeightUnit.Pound:
                return 0.453592;

                //Invalid Unit
                default:
                throw new ArgumentException("Invalid Weight Unit");
            }
        }       


        //Method to convert to base unit
        public static double ConvertToBase(double value,WeightUnit unit)
        {
            return value*GetConversionFactor(unit);
        }

        //Method to convert from base unit to target unit
        public static double ConvertFromBase(double baseValue,WeightUnit target)
        {
            return baseValue/GetConversionFactor(target);
        }
    }
}