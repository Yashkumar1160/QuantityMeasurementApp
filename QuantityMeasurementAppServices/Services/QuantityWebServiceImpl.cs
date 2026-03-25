using System.Collections.Generic;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppServices.Interfaces;

namespace QuantityMeasurementAppServices.Services
{
    public class QuantityWebServiceImpl : IQuantityWebService
    {
        private readonly IQuantityMeasurementService service;
        private readonly IQuantityRecordRepository repository;

        // Constructor
        public QuantityWebServiceImpl(IQuantityMeasurementService service, IQuantityRecordRepository repository)
        {
            this.service = service;
            this.repository = repository;
        }

        // Method for comparison
        public QuantityMeasurementResponseDTO Compare(QuantityInputRequest request)
        {
            bool result = service.Compare(request.ThisQuantityDTO, request.ThatQuantityDTO);

            QuantityMeasurementResponseDTO dto = new QuantityMeasurementResponseDTO
            {
                ThisValue = request.ThisQuantityDTO.Value,
                ThisUnit = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType = request.ThisQuantityDTO.MeasurementType,
                ThatValue = request.ThatQuantityDTO.Value,
                ThatUnit = request.ThatQuantityDTO.UnitName,
                ThatMeasurementType = request.ThatQuantityDTO.MeasurementType,
                Operation = "Compare",
                ResultString = result.ToString(),
                IsError = false
            };
            return dto;
        }

        // Method for Conversion
        public QuantityMeasurementResponseDTO Convert(ConvertRequest request)
        {
            QuantityDTO result = service.Convert(request.ThisQuantityDTO, request.TargetUnit);

            QuantityMeasurementResponseDTO dto = new QuantityMeasurementResponseDTO
            {
                ThisValue = request.ThisQuantityDTO.Value,
                ThisUnit = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType = request.ThisQuantityDTO.MeasurementType,
                Operation = "Convert",
                ResultValue = result.Value,
                ResultUnit = request.TargetUnit,
                IsError = false
            };
            return dto;
        }

        // Method for Addition
        public QuantityMeasurementResponseDTO Add(ArithmeticRequest request)
        {
            QuantityDTO result = service.Add(request.ThisQuantityDTO, request.ThatQuantityDTO, request.TargetUnit);

            QuantityMeasurementResponseDTO dto = new QuantityMeasurementResponseDTO
            {
                ThisValue = request.ThisQuantityDTO.Value,
                ThisUnit = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType = request.ThisQuantityDTO.MeasurementType,
                ThatValue = request.ThatQuantityDTO.Value,
                ThatUnit = request.ThatQuantityDTO.UnitName,
                ThatMeasurementType = request.ThatQuantityDTO.MeasurementType,
                Operation = "Add",
                ResultValue = result.Value,
                ResultUnit = request.TargetUnit,
                ResultMeasurementType = request.ThisQuantityDTO.MeasurementType,
                IsError = false
            };
            return dto;
        }

        // Method for Subtraction
        public QuantityMeasurementResponseDTO Subtract(ArithmeticRequest request)
        {
            QuantityDTO result = service.Subtract(request.ThisQuantityDTO, request.ThatQuantityDTO, request.TargetUnit);

            QuantityMeasurementResponseDTO dto = new QuantityMeasurementResponseDTO
            {
                ThisValue = request.ThisQuantityDTO.Value,
                ThisUnit = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType = request.ThisQuantityDTO.MeasurementType,
                ThatValue = request.ThatQuantityDTO.Value,
                ThatUnit = request.ThatQuantityDTO.UnitName,
                ThatMeasurementType = request.ThatQuantityDTO.MeasurementType,
                Operation = "Subtract",
                ResultValue = result.Value,
                ResultUnit = request.TargetUnit,
                ResultMeasurementType = request.ThisQuantityDTO.MeasurementType,
                IsError = false
            };
            return dto;
        }

        // Method for Division
        public QuantityMeasurementResponseDTO Divide(QuantityInputRequest request)
        {
            double result = service.Divide(request.ThisQuantityDTO, request.ThatQuantityDTO);

            QuantityMeasurementResponseDTO dto = new QuantityMeasurementResponseDTO
            {
                ThisValue = request.ThisQuantityDTO.Value,
                ThisUnit = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType = request.ThisQuantityDTO.MeasurementType,
                ThatValue = request.ThatQuantityDTO.Value,
                ThatUnit = request.ThatQuantityDTO.UnitName,
                ThatMeasurementType = request.ThatQuantityDTO.MeasurementType,
                Operation = "Divide",
                ResultValue = result,
                IsError = false
            };
            return dto;
        }

        // Method to get history by operations
        public List<QuantityMeasurementResponseDTO> GetHistoryByOperation(string operation)
        {
            return QuantityMeasurementResponseDTO.FromEntityList(repository.GetByOperation(operation));
        }

        // Method to get history by measurement type
        public List<QuantityMeasurementResponseDTO> GetHistoryByType(string measurementType)
        {
            return QuantityMeasurementResponseDTO.FromEntityList(repository.GetByMeasurementType(measurementType));
        }

        // Method to get error history
        public List<QuantityMeasurementResponseDTO> GetErrorHistory()
        {
            return QuantityMeasurementResponseDTO.FromEntityList(repository.GetErrorHistory());
        }

        // Method to get total operations count
        public int GetOperationCount(string operation)
        {
            return repository.GetOperationCount(operation);
        }
    }
}