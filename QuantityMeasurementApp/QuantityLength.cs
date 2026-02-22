using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class QuantityLength
    {
        //Encapsulated Fields
        private readonly double value;
        private readonly LengthUnit unit;

        //Epsilon value for floating point comparison
        private const double epsilon = 0.0001;

        //Constructor
        public QuantityLength(double value, LengthUnit unit)
        {
            //Check unit type
            if (!Enum.IsDefined(typeof(LengthUnit), unit))
            {
                throw new ArgumentException("Invalid unit type");
            }

            //Check numeric type
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid numeric value");
            }

            this.value = value;
            this.unit = unit;
        }


        //Method to get conversion factor
        private static double GetConversionFactor(LengthUnit unit)
        {
            //FEET
            if (unit == LengthUnit.Feet)
            {
                return 1.0;
            }

            //INCH
            if (unit == LengthUnit.Inch)
            {
                return 1.0 / 12.0;

            }

            //YARD
            if (unit == LengthUnit.Yard)
            {
                return 3.0;
            }

            //CENTIMETER
            if (unit == LengthUnit.Centimeter)
            {
                return 0.0328084;
            }

            throw new ArgumentException("Invalid Unit");
        }


        //Method to convert current object to base unit
        private double ConvertToBaseUnit()
        {
            return ConvertToBase(this.value, this.unit);
        }

        //Method to Convert any value to base unit
        private static double ConvertToBase(double value, LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.Feet:
                    return value;

                case LengthUnit.Inch:
                    return value / 12.0;

                case LengthUnit.Yard:
                    return value * 3.0;

                case LengthUnit.Centimeter:
                    return value * 0.0328084;

                default:
                    throw new ArgumentException("Invalid Unit Type");

            }
        }
        //Method to Convert from base unit (Feet) to target unit
        private static double ConvertFromBase(double valueInFeet, LengthUnit target)
        {
            switch (target)
            {
                case LengthUnit.Feet:
                    return valueInFeet;

                case LengthUnit.Inch:
                    return valueInFeet * 12.0;

                case LengthUnit.Yard:
                    return valueInFeet / 3.0;

                case LengthUnit.Centimeter:
                    return valueInFeet / 0.0328084;

                default:
                    throw new ArgumentException("Invalid unit type");
            }
        }

        //Static Method To Convert From Source Unit To Target Unit
        public static double Convert(double value, LengthUnit sourceUnit, LengthUnit targetUnit)
        {
            //Check if numeric value is valid
            if (!double.IsFinite(value))
            {
                throw new ArgumentException("Value must be finite");
            }
            //Check if units are valid
            if (!Enum.IsDefined(typeof(LengthUnit), sourceUnit) || !Enum.IsDefined(typeof(LengthUnit), targetUnit))
            {
                throw new ArgumentException("Invalid unit type");
            }

            //Convert source to base unit (Feet)
            double valueInFeet = ConvertToBase(value, sourceUnit);

            //Convert base unit to target
            return ConvertFromBase(valueInFeet, targetUnit);
        }

        //Instance ConvertTo  Method 
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            //Check if unit is valid
            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
            {
                throw new ArgumentException("Invalid target unit");
            }

            double baseValue = ConvertToBaseUnit();

            double convertedValue = ConvertFromBase(baseValue, targetUnit);

            return new QuantityLength(convertedValue, targetUnit);
        }

        //Override Equals method 
        public override bool Equals(object obj)
        {
            //Check same reference
            if (this == obj)
            {
                return true;
            }


            //Check for different type and null
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            //Safe Type Casting
            QuantityLength other = (QuantityLength)obj;

            //Compare Values 
            return Math.Abs(this.ConvertToBaseUnit() - other.ConvertToBaseUnit()) <= epsilon;
        }

        //Override GetHashCode method
        public override int GetHashCode()
        {
            return ConvertToBaseUnit().GetHashCode();
        }
    }
}