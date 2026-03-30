using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppServices.Interfaces;

namespace QuantityMeasurementAppServices.Services
{
    public class QuantityWebServiceImpl : IQuantityWebService
    {
        // service and repo instances
        private readonly IQuantityMeasurementService service;
        private readonly IQuantityRecordRepository repository;

        // Constructor

        public QuantityWebServiceImpl(IQuantityMeasurementService service, IQuantityRecordRepository repository)
        {
            this.service = service;
            this.repository = repository;
        }

        // Method to Compare two quantities - result saved to DB under the calling user's ID
        public QuantityMeasurementResponseDTO Compare(QuantityInputRequest request, long userId)
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

            // Persist the operation record linked to this user
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                userId,
                "Compare",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                request.ThatQuantityDTO.Value, request.ThatQuantityDTO.UnitName,
                result ? 1 : 0,
                request.ThisQuantityDTO.MeasurementType
            );
            repository.Save(entity);

            return dto;
        }

        // Method to Convert a quantity to a different unit - result saved to DB under the calling user's ID
        public QuantityMeasurementResponseDTO Convert(ConvertRequest request, long userId)
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

            // Persist the operation record linked to this user
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                userId,
                "Convert",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                result.Value,
                request.ThisQuantityDTO.MeasurementType
            );
            repository.Save(entity);

            return dto;
        }

        // Method to Add two quantities - result saved to DB under the calling user's ID
        public QuantityMeasurementResponseDTO Add(ArithmeticRequest request, long userId)
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

            // Persist the operation record linked to this user
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                userId,
                "Add",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                request.ThatQuantityDTO.Value, request.ThatQuantityDTO.UnitName,
                result.Value,
                request.ThisQuantityDTO.MeasurementType
            );
            repository.Save(entity);

            return dto;
        }

        // Method to Subtract one quantity from another - result saved to DB under the calling user's ID
        public QuantityMeasurementResponseDTO Subtract(ArithmeticRequest request, long userId)
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

            // Persist the operation record linked to this user
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                userId,
                "Subtract",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                request.ThatQuantityDTO.Value, request.ThatQuantityDTO.UnitName,
                result.Value,
                request.ThisQuantityDTO.MeasurementType
            );
            repository.Save(entity);

            return dto;
        }

        // Method to Divide one quantity by another - result saved to DB under the calling user's ID
        public QuantityMeasurementResponseDTO Divide(QuantityInputRequest request, long userId)
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

            // Persist the operation record linked to this user
            QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
                userId,
                "Divide",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                request.ThatQuantityDTO.Value, request.ThatQuantityDTO.UnitName,
                result,
                request.ThisQuantityDTO.MeasurementType
            );
            repository.Save(entity);

            return dto;
        }

        // Method to Get history filtered by operation type - only this user's records
        public List<QuantityMeasurementResponseDTO> GetHistoryByOperation(string operation, long userId)
        {
            return QuantityMeasurementResponseDTO.FromEntityList(
                repository.GetByOperation(operation, userId));
        }

        // Method to Get history filtered by measurement type - only this user's records
        public List<QuantityMeasurementResponseDTO> GetHistoryByType(string measurementType, long userId)
        {
            return QuantityMeasurementResponseDTO.FromEntityList(
                repository.GetByMeasurementType(measurementType, userId));
        }

        // Method to Get error history - only this user's records
        public List<QuantityMeasurementResponseDTO> GetErrorHistory(long userId)
        {
            return QuantityMeasurementResponseDTO.FromEntityList(
                repository.GetErrorHistory(userId));
        }

        // Method to Get count of successful operations - only this user's records
        public int GetOperationCount(string operation, long userId)
        {
            return repository.GetOperationCount(operation, userId);
        }
    }
}