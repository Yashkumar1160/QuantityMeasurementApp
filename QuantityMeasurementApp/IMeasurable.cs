using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementApp
{
    public interface IMeasurable
    {
        //Method to Get Conversion Factor
        double GetConversionFactor();

        //Method to Convert To Base Unit
        double ConvertToBaseUnit(double value);

        //Method to Convert From Base Unit 
        double ConvertFromBaseUnit(double baseValue);

        //Method to Get Unit Name
        string GetUnitName();
    }
}