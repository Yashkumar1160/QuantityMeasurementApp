using System.Collections.Generic;
using QuantityMeasurementAppModels.DTOs;

namespace QuantityMeasurementAppServices.Interfaces
{
    public interface IQuantityWebService
    {
        QuantityMeasurementResponseDTO Compare(QuantityInputRequest request, long userId);
        QuantityMeasurementResponseDTO Convert(ConvertRequest request, long userId);
        QuantityMeasurementResponseDTO Add(ArithmeticRequest request, long userId);
        QuantityMeasurementResponseDTO Subtract(ArithmeticRequest request, long userId);
        QuantityMeasurementResponseDTO Divide(QuantityInputRequest request, long userId);
        List<QuantityMeasurementResponseDTO> GetHistoryByOperation(string operation, long userId);
        List<QuantityMeasurementResponseDTO> GetHistoryByType(string measurementType, long userId);
        List<QuantityMeasurementResponseDTO> GetErrorHistory(long userId);
        List<QuantityMeasurementResponseDTO> GetAllHistory(long userId);     // NEW
        int GetOperationCount(string operation, long userId);
    }
}