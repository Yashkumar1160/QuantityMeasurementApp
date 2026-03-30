using System;
using System.Collections.Generic;
using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppRepositories.Interfaces
{
    public interface IQuantityRecordRepository
    {
        void Save(QuantityMeasurementEntity entity);

        List<QuantityMeasurementEntity> GetAll(long userId);

        List<QuantityMeasurementEntity> GetByOperation(string operation, long userId);

        List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType, long userId);

        List<QuantityMeasurementEntity> GetErrorHistory(long userId);

        int GetOperationCount(string operation, long userId);

        List<QuantityMeasurementEntity> GetByCreatedAfter(DateTime date, long userId);
    }
}