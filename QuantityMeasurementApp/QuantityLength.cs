using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class QuantityLength
    {
        //Encapsulated Fields
        private readonly double Value;
        private readonly LengthUnit Unit;

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

            this.Value = value;
            this.Unit = unit;
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
            double valueInFeet = ExtendedLengthUnit.ConvertToBase(value, sourceUnit);

            //Convert base unit to target
            return ExtendedLengthUnit.ConvertFromBase(valueInFeet, targetUnit);
        }

        //Instance ConvertTo  Method 
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            //Check if unit is valid
            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
            {
                throw new ArgumentException("Invalid target unit");
            }

            double baseValue = ExtendedLengthUnit.ConvertToBase(this.Value,this.Unit);

            double convertedValue = ExtendedLengthUnit.ConvertFromBase(baseValue, targetUnit);

            return new QuantityLength(convertedValue, targetUnit);
        }

        //Method to Add Two Length Units (UC-6)
        public QuantityLength Add(QuantityLength second)
        {
            //Check null
            if (second == null)
            {
                throw new ArgumentException("Second operand cannot be null");
            }

            //Convert both to base unit (Feet)
            double firstInFeet = ExtendedLengthUnit.ConvertToBase(this.Value,this.Unit);
            double secondInFeet = ExtendedLengthUnit.ConvertToBase(second.Value,second.Unit);

            //Add values in base unit
            double sumInFeet = firstInFeet + secondInFeet;

            //Convert result back to FIRST operand unit
            double resultValue = ExtendedLengthUnit.ConvertFromBase(sumInFeet, this.Unit);

            //Return new object (immutability)
            return new QuantityLength(resultValue, this.Unit);
        }

        //Method to Add Two Lengths with Target Unit (UC-7)
        public QuantityLength Add(QuantityLength second, LengthUnit targetUnit)
        {
            //Check null
            if (second == null)
            {
                throw new ArgumentException("Second operand cannot be null");
            }

            //Check target unit
            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
            {
                throw new ArgumentException("Invalid target unit");
            }

            //Convert both to base unit (Feet)
            double firstInFeet = ExtendedLengthUnit.ConvertToBase(this.Value,this.Unit);
            double secondInFeet = ExtendedLengthUnit.ConvertToBase(second.Value,second.Unit);

            //Add in base unit
            double sumInFeet = firstInFeet + secondInFeet;

            //Convert result to specified target unit
            double resultValue = ExtendedLengthUnit.ConvertFromBase(sumInFeet, targetUnit);

            return new QuantityLength(resultValue, targetUnit);
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
            return Math.Abs(ExtendedLengthUnit.ConvertToBase(this.Value,this.Unit) - ExtendedLengthUnit.ConvertToBase(other.Value,other.Unit)) <= epsilon;
        }

        //Override GetHashCode method
        public override int GetHashCode()
        {
            //Round value according to epsilon before hashing
            double baseValue = ExtendedLengthUnit.ConvertToBase(this.Value,this.Unit);
            double rounded = Math.Round(baseValue / epsilon) * epsilon;
            return rounded.GetHashCode();
        }

        //Override ToString method
        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}