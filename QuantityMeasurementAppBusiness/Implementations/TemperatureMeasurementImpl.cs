using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuantityMeasurementAppBusiness.Interfaces;
using QuantityMeasurementAppModels.Enums;

namespace QuantityMeasurementAppBusiness.Implementations
{
    public class TemperatureMeasurementImpl : IMeasurable
    {
        private TemperatureUnit unit;

        //Constructor
        public TemperatureMeasurementImpl(TemperatureUnit unitType)
        {
            unit = unitType;
        }

        //Temperature does not use simple conversion factor
        public double GetConversionFactor()
        {
            return 1.0;
        }

        //Convert value to base unit (Celsius)
        public double ConvertToBaseUnit(double value)
        {
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return value;

                case TemperatureUnit.Fahrenheit:
                    return (value - 32) * 5 / 9;

                case TemperatureUnit.Kelvin:
                    return value - 273.15;

                default:
                    throw new ArgumentException("Invalid Temperature Unit");
            }
        }

        //Convert base unit (Celsius) to target unit
        public double ConvertFromBaseUnit(double baseValue)
        {
            switch (unit)
            {
                case TemperatureUnit.Celsius:
                    return baseValue;

                case TemperatureUnit.Fahrenheit:
                    return (baseValue * 9 / 5) + 32;

                case TemperatureUnit.Kelvin:
                    return baseValue + 273.15;

                default:
                    throw new ArgumentException("Invalid Temperature Unit");
            }
        }

        //Return unit name
        public string GetUnitName()
        {
            return unit.ToString();
        }

        //Temperature does not support arithmetic
        public bool SupportsArithmetic()
        {
            return false;
        }

        //Throw exception when arithmetic is attempted
        public void ValidateOperationSupport(string operation)
        {
            throw new NotSupportedException("Temperature does not support " + operation + " operation");
        }

        //Method to get measurement type 
        public string GetMeasurementType()
        {
            return "Temperature";
        }

        //Method to get unit by name
        public IMeasurable GetUnitByName(string unitName)
        {
            TemperatureUnit parsedUnit = (TemperatureUnit)Enum.Parse(typeof(TemperatureUnit), unitName, true);
            return new TemperatureMeasurementImpl(parsedUnit);
        }
    }
}