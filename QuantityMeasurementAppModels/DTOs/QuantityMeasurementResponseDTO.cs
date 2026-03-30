using System.Collections.Generic;
using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppModels.DTOs
{
    public class QuantityMeasurementResponseDTO
    {
        // Properties
        public double ThisValue { get; set; }
        public string? ThisUnit { get; set; }
        public string? ThisMeasurementType { get; set; }
        public double ThatValue { get; set; }
        public string? ThatUnit { get; set; }
        public string? ThatMeasurementType { get; set; }
        public string? Operation { get; set; }
        public string? ResultString { get; set; }
        public double ResultValue { get; set; }
        public string? ResultUnit { get; set; }
        public string? ResultMeasurementType { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsError { get; set; }

        // Convert one entity to one DTO
        public static QuantityMeasurementResponseDTO FromEntity(QuantityMeasurementEntity entity)
        {
            QuantityMeasurementResponseDTO dto = new QuantityMeasurementResponseDTO();

            dto.ThisValue           = entity.FirstValue;
            dto.ThisUnit            = entity.FirstUnit;
            dto.ThisMeasurementType = entity.MeasurementType;
            dto.ThatValue           = entity.SecondValue;
            dto.ThatUnit            = entity.SecondUnit;
            dto.ThatMeasurementType = entity.MeasurementType;
            dto.Operation           = entity.Operation;
            dto.ResultValue         = entity.ResultValue;
            dto.ErrorMessage        = entity.ErrorMessage;
            dto.IsError             = entity.IsError;

            return dto;
        }

        // Convert a list of entities to a list of DTOs
        public static List<QuantityMeasurementResponseDTO> FromEntityList(List<QuantityMeasurementEntity> entities)
        {
            List<QuantityMeasurementResponseDTO> list = new List<QuantityMeasurementResponseDTO>();

            for (int i = 0; i < entities.Count; i++)
            {
                list.Add(FromEntity(entities[i]));
            }

            return list;
        }

        // Convert this DTO back to an Entity.
        // UserId is passed in because the DTO does not hold it — the caller (service layer) owns it.
        public QuantityMeasurementEntity ToEntity(long userId)
        {
            if (IsError)
            {
                return new QuantityMeasurementEntity(userId, Operation!, ErrorMessage!);
            }

            return new QuantityMeasurementEntity(
                userId,
                Operation!,
                ThisValue, ThisUnit!,
                ThatValue, ThatUnit!,
                ResultValue,
                ThisMeasurementType!);
        }

        // Convert a list of DTOs back to entities
        public static List<QuantityMeasurementEntity> ToEntityList(List<QuantityMeasurementResponseDTO> dtos, long userId)
        {
            List<QuantityMeasurementEntity> list = new List<QuantityMeasurementEntity>();

            for (int i = 0; i < dtos.Count; i++)
            {
                list.Add(dtos[i].ToEntity(userId));
            }

            return list;
        }
    }
}