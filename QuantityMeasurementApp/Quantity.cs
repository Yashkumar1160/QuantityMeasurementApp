using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class Quantity<U> where U : IMeasurable
    {
        //Encapsulated Fields
        private readonly double Value;
        private readonly U Unit;

        //Epsilon value for floating point comparison
        private const double epsilon = 0.0001;

        //Constructor
        public Quantity(double value, U unit)
        {
            //Check if unit is null
            if (unit == null)
            {
                throw new ArgumentException("Unit cannot be null");
            }

            //Check numeric type
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid numeric value");
            }

            this.Value = value;
            this.Unit = unit;
        }


        //Instance ConvertTo Method (Convert quantity to target unit)
        public Quantity<U> ConvertTo(U targetUnit)
        {
            //Check if target unit is null
            if (targetUnit == null)
            {
                throw new ArgumentException("Invalid target unit");
            }

            //Convert current value to base unit
            double baseValue = Unit.ConvertToBaseUnit(this.Value);

            //Convert base unit to target unit
            double convertedValue = targetUnit.ConvertFromBaseUnit(baseValue);

            return new Quantity<U>(convertedValue, targetUnit);
        }


        //Method to Add Two Quantities (UC-10)
        public Quantity<U> Add(Quantity<U> second)
        {
            //Check null
            if (second == null)
            {
                throw new ArgumentException("Second operand cannot be null");
            }

            //Convert both quantities to base unit
            double firstBase = Unit.ConvertToBaseUnit(this.Value);
            double secondBase = second.Unit.ConvertToBaseUnit(second.Value);

            //Add values in base unit
            double sumBase = firstBase + secondBase;

            //Convert result back to FIRST operand unit
            double resultValue = Unit.ConvertFromBaseUnit(sumBase);

            return new Quantity<U>(resultValue, this.Unit);
        }


        //Method to Add Two Quantities with Target Unit
        public Quantity<U> Add(Quantity<U> second, U targetUnit)
        {
            //Check null
            if (second == null)
            {
                throw new ArgumentException("Second operand cannot be null");
            }

            //Check target unit
            if (targetUnit == null)
            {
                throw new ArgumentException("Invalid target unit");
            }

            //Convert both quantities to base unit
            double firstBase = Unit.ConvertToBaseUnit(this.Value);
            double secondBase = second.Unit.ConvertToBaseUnit(second.Value);

            //Add in base unit
            double sumBase = firstBase + secondBase;

            //Convert result to specified target unit
            double resultValue = targetUnit.ConvertFromBaseUnit(sumBase);

            return new Quantity<U>(resultValue, targetUnit);
        }


        //Override Equals method
        public override bool Equals(object obj)
        {
            //Check same reference
            if (this == obj)
            {
                return true;
            }

            //Check for null or different type
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            //Safe type casting
            Quantity<U> other = (Quantity<U>)obj;

            //Check if both quantities belong to same measurement category
            if (Unit.GetType() != other.Unit.GetType())
            {
                return false;
            }

            //Convert both values to base unit
            double firstBase = Unit.ConvertToBaseUnit(this.Value);
            double secondBase = other.Unit.ConvertToBaseUnit(other.Value);

            //Compare values using epsilon tolerance
            return Math.Abs(firstBase - secondBase) <= epsilon;
        }


        //Override GetHashCode method
        public override int GetHashCode()
        {
            //Round value according to epsilon before hashing
            double baseValue = Unit.ConvertToBaseUnit(this.Value);
            double rounded = Math.Round(baseValue / epsilon) * epsilon;

            return rounded.GetHashCode();
        }


        //Override ToString method
        public override string ToString()
        {
            return $"{Value} {Unit.GetUnitName()}";
        }

        //Method to get value
        public double GetValue()
        {
            return Value;
        }
    }
}