using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public class QuantityWeight
    {
        //Encapsulated Fields
        private readonly double Value;
        private readonly WeightUnit Unit;

        //Epsilon value for floating point comparison
        private const double epsilon = 0.0001;

        //Constructor
        public QuantityWeight(double value, WeightUnit unit)
        {
            //Check if unit is valid
            if (!Enum.IsDefined(typeof(WeightUnit), unit))
            {
                throw new ArgumentException("Invalid weight unit");
            }
            //Check if value is valid
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Invalid numeric value");
            }

            this.Value = value;
            this.Unit = unit;
        }

        //Static Method To Convert From Source Unit To Target Unit
        public static double Convert(double value, WeightUnit sourceUnit, WeightUnit targetUnit)
        {
            //Check if numeric value is valid
            if (!double.IsFinite(value))
            {
                throw new ArgumentException("Value must be finite");
            }
            //Check if units are valid
            if (!Enum.IsDefined(typeof(WeightUnit), sourceUnit) || !Enum.IsDefined(typeof(WeightUnit), targetUnit))
            {
                throw new ArgumentException("Invalid unit type");
            }

            //Convert source to base unit (Kilogram)
            double valueInKg = ExtendedWeightUnit.ConvertToBase(value, sourceUnit);

            //Convert base unit to target
            return ExtendedWeightUnit.ConvertFromBase(valueInKg, targetUnit);
        }

        //Instance ConvertTo Method
        public QuantityWeight ConvertTo(WeightUnit target)
        {
            //Check if unit is valid
            if (!Enum.IsDefined(typeof(WeightUnit), target))
            {
                throw new ArgumentException("Invalid target unit");
            }

            double baseValue = ExtendedWeightUnit.ConvertToBase(this.Value, this.Unit);

            double convertedValue = ExtendedWeightUnit.ConvertFromBase(baseValue, target);

            return new QuantityWeight(convertedValue, target);
        }

        //Method to Add Two Weight Units 
        public QuantityWeight Add(QuantityWeight second)
        {
            //Check null
            if (second == null)
                throw new ArgumentException("Second weight cannot be null");

            //Convert both to base unit (Kilogram)
            double firstInKg = ExtendedWeightUnit.ConvertToBase(this.Value, this.Unit);
            double secondInKg = ExtendedWeightUnit.ConvertToBase(second.Value, second.Unit);

            //Add values in base unit
            double sumInKg = firstInKg + secondInKg;

            //Convert result back to FIRST operand unit
            double result = ExtendedWeightUnit.ConvertFromBase(sumInKg, this.Unit);

            //Return new object 
            return new QuantityWeight(result, this.Unit);
        }

        //Method to Add Two Weights with Target Unit 
        public QuantityWeight Add(QuantityWeight second, WeightUnit target)
        {
            //Check null
            if (second == null)
                throw new ArgumentException("Second weight cannot be null");

            //Check target unit
            if (!Enum.IsDefined(typeof(WeightUnit), target))
            {
                throw new ArgumentException("Invalid target unit");
            }

            //Convert both to base unit (Kilogram)
            double firstInKg = ExtendedWeightUnit.ConvertToBase(this.Value, this.Unit);
            double secondInKg = ExtendedWeightUnit.ConvertToBase(second.Value, second.Unit);

            //Add in base unit
            double sumInKg = firstInKg + secondInKg;

            //Convert result to specified target unit
            double result = ExtendedWeightUnit.ConvertFromBase(sumInKg, target);

            return new QuantityWeight(result, target);
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
            QuantityWeight other = (QuantityWeight)obj;

            //Compare Values 
            return Math.Abs(ExtendedWeightUnit.ConvertToBase(this.Value, this.Unit) - ExtendedWeightUnit.ConvertToBase(other.Value, other.Unit)) <= epsilon;
        }

        //Override GetHashCode method
        public override int GetHashCode()
        {
            //Round value according to epsilon before hashing
            double baseValue = ExtendedWeightUnit.ConvertToBase(this.Value, this.Unit);
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
