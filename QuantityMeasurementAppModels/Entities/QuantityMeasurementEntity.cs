using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementAppModels.Entities
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        //Properties
        public string Operation { get; set; }
        public double FirstValue { get; set; }
        public string FirstUnit { get; set; }
        public double SecondValue { get; set; }
        public string SecondUnit { get; set; }
        public double ResultValue { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }

        //Constructor for Two-operand operations: Add, Subtract, Compare, Divide
        public QuantityMeasurementEntity(string operation, double firstValue, string firstUnit, double secondValue, string secondUnit, double resultValue)
        {
            Operation = operation;
            FirstValue = firstValue;
            FirstUnit = firstUnit;
            SecondValue = secondValue;
            SecondUnit = secondUnit;
            ResultValue = resultValue;
            IsError = false;
        }

        //Constructor for Single-operand operations: Convert
        public QuantityMeasurementEntity(string operation, double inputValue, string inputUnit, double resultValue)
        {
            Operation = operation;
            FirstValue = inputValue;
            FirstUnit = inputUnit;
            ResultValue = resultValue;
            IsError = false;
        }

        //Constructor for Error case
        public QuantityMeasurementEntity(string operation, string errorMessage)
        {
            Operation = operation;
            IsError = true;
            ErrorMessage = errorMessage;
        }

        //Override ToString Method
        public override string ToString()
        {
            if (IsError)
            {
                return $"[{Operation}] ERROR: {ErrorMessage}";
            }

            return $"[{Operation}] {FirstValue} {FirstUnit} → {ResultValue}";
        }
    }
}