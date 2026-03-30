using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantityMeasurementAppBusiness.Interfaces
{
    public interface IMeasurable
    {
        // Method to Get Conversion Factor
        double GetConversionFactor();

        // Method to Convert To Base Unit
        double ConvertToBaseUnit(double value);

        // Method to Convert From Base Unit 
        double ConvertFromBaseUnit(double baseValue);

        // Method to Get Unit Name
        string GetUnitName();

        // UC-14  Temperature Measurement with Selective Arithmetic Support and IMeasurable Refactoring

        // Method to check if arithmetic operations are supported
        bool SupportsArithmetic();

        // Method to validate arithmetic operation
        void ValidateOperationSupport(string operation);

        // Method to get measurement type
        string GetMeasurementType();    

        // Method to get unit by name    
        IMeasurable GetUnitByName(string unitName);  
    }
}