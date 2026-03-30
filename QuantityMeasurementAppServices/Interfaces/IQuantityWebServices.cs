using System.Collections.Generic;
using QuantityMeasurementAppModels.DTOs;

namespace QuantityMeasurementAppServices.Interfaces
{
    public interface IQuantityWebService
    {
        // Method for comparison
        QuantityMeasurementResponseDTO Compare(QuantityInputRequest request, long userId);

        // Method for conversion
        QuantityMeasurementResponseDTO Convert(ConvertRequest request, long userId);

        // Method for addition
        QuantityMeasurementResponseDTO Add(ArithmeticRequest request, long userId);

        // Method for subtraction
        QuantityMeasurementResponseDTO Subtract(ArithmeticRequest request, long userId);

        // Method for division
        QuantityMeasurementResponseDTO Divide(QuantityInputRequest request, long userId);

        // Method to get history by operation
        List<QuantityMeasurementResponseDTO> GetHistoryByOperation(string operation, long userId);

        // Method to get history by type
        List<QuantityMeasurementResponseDTO> GetHistoryByType(string measurementType, long userId);

        // Method to get error history
        List<QuantityMeasurementResponseDTO> GetErrorHistory(long userId);

        // Method to get total operation count
        int GetOperationCount(string operation, long userId);
    }
}