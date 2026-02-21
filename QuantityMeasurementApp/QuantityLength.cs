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

        //Constructor
        public QuantityLength(double value, LengthUnit unit)
        {
            if (!Enum.IsDefined(typeof(LengthUnit), unit))
                throw new ArgumentException("Invalid unit type");

            this.value = value;
            this.unit = unit;
        }

        //Method to convert everything to base unit
        private double ConvertToBaseUnit()
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
            return Math.Abs(this.ConvertToBaseUnit() - other.ConvertToBaseUnit()) <=0.0001;
        }

        //Override GetHashCode method
        public override int GetHashCode()
        {
            return this.ConvertToBaseUnit().GetHashCode();
        }
    }
}