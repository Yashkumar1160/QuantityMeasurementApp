using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppModels.Enums;

namespace QuantityMeasurementAppBusiness
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


        //UC-13 Centralized Arithmetic Logic to Enforce DRY in Quantity Operations

        //Method to validate arithmetic operands
        private void ValidateArithmeticOperands(Quantity<U> second, U targetUnit, bool targetRequired)
        {
            //Check null
            if (second == null)
            {
                throw new ArgumentException("Second operand cannot be null");
            }

            //Check category mismatch
            if (Unit.GetType() != second.Unit.GetType())
            {
                throw new ArgumentException("Cannot perform operation on different measurement categories");
            }

            //Check numeric values
            if (double.IsNaN(Value) || double.IsInfinity(Value) ||
                double.IsNaN(second.Value) || double.IsInfinity(second.Value))
            {
                throw new ArgumentException("Invalid numeric value");
            }

            //Check target unit if required
            if (targetRequired && targetUnit == null)
            {
                throw new ArgumentException("Invalid target unit");
            }
        }


        //Method to perform arithmetic in base unit
        private double PerformBaseArithmetic(Quantity<U> second, ArithmeticOperation operation)
        {
            //Convert both quantities to base unit
            double firstBase = Unit.ConvertToBaseUnit(this.Value);
            double secondBase = second.Unit.ConvertToBaseUnit(second.Value);

            //Perform arithmetic operation
            if (operation == ArithmeticOperation.ADD)
            {
                return firstBase + secondBase;
            }
            else if (operation == ArithmeticOperation.SUBTRACT)
            {
                return firstBase - secondBase;
            }
            else if (operation == ArithmeticOperation.DIVIDE)
            {
                return firstBase / secondBase;
            }

            throw new ArgumentException("Invalid arithmetic operation");
        }


        //Method to Add Two Quantities
        public Quantity<U> Add(Quantity<U> second)
        {
            //Check arithmetic support
            Unit.ValidateOperationSupport("Addition");

            //Validate operands
            ValidateArithmeticOperands(second, this.Unit, true);

            double baseResult = PerformBaseArithmetic(second, ArithmeticOperation.ADD);

            double resultValue = Unit.ConvertFromBaseUnit(baseResult);

            return new Quantity<U>(resultValue, this.Unit);
        }


        //Method to Add Two Quantities with Target Unit
        public Quantity<U> Add(Quantity<U> second, U targetUnit)
        {
            //Check arithmetic support
            Unit.ValidateOperationSupport("Addition");

            //Validate operands
            ValidateArithmeticOperands(second, targetUnit, true);

            double baseResult = PerformBaseArithmetic(second, ArithmeticOperation.ADD);

            double resultValue = targetUnit.ConvertFromBaseUnit(baseResult);

            return new Quantity<U>(resultValue, targetUnit);
        }

        //UC-12 Subtraction and Division Operations on Quantity Measurements

        //Method to Subtract Two Quantities
        public Quantity<U> Subtract(Quantity<U> second)
        {
            Unit.ValidateOperationSupport("Subtraction");

            ValidateArithmeticOperands(second, this.Unit, true);

            double baseResult = PerformBaseArithmetic(second, ArithmeticOperation.SUBTRACT);

            double resultValue = Unit.ConvertFromBaseUnit(baseResult);

            resultValue = Math.Round(resultValue, 2);

            return new Quantity<U>(resultValue, this.Unit);
        }

        //Method to Subtract Two Quantities with Target Unit
        public Quantity<U> Subtract(Quantity<U> second, U targetUnit)
        {
            //Check arithmetic support
            Unit.ValidateOperationSupport("Subtraction");

            //Validate operands
            ValidateArithmeticOperands(second, targetUnit, true);

            double baseResult = PerformBaseArithmetic(second, ArithmeticOperation.SUBTRACT);

            double resultValue = targetUnit.ConvertFromBaseUnit(baseResult);

            resultValue = Math.Round(resultValue, 2);

            return new Quantity<U>(resultValue, targetUnit);
        }

        //Method to Divide Two Quantities
        public double Divide(Quantity<U> second)
        {
            Unit.ValidateOperationSupport("Division");

            ValidateArithmeticOperands(second, this.Unit, false);

            if (second.Value == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero quantity");
            }

            return PerformBaseArithmetic(second, ArithmeticOperation.DIVIDE);
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