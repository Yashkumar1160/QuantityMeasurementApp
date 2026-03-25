using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QuantityMeasurementAppModels.DTOs;

namespace QuantityMeasurementAppServices.Interfaces
{
    public interface IQuantityMeasurementService
    {
        //Method for Addition
        QuantityDTO Add(QuantityDTO first, QuantityDTO second, string targetUnit);

        //Method for Subtraction
        QuantityDTO Subtract(QuantityDTO first, QuantityDTO second, string targetUnit);

        //Method for Division
        double Divide(QuantityDTO first, QuantityDTO second);

        //Method for Comparison
        bool Compare(QuantityDTO first, QuantityDTO second);

        //Method for Convert
        QuantityDTO Convert(QuantityDTO quantity, string targetUnit);
    }
}