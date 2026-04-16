using System;
using System.Collections.Generic;
using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppRepositories.Interfaces
{
    public interface IQuantityRecordRepository
    {
        System.Threading.Tasks.Task SaveAsync(QuantityMeasurementEntity entity);

        System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetAllAsync(long userId);

        System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetByOperationAsync(string operation, long userId);

        System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetByMeasurementTypeAsync(string measurementType, long userId);

        System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetErrorHistoryAsync(long userId);

        System.Threading.Tasks.Task<int> GetOperationCountAsync(string operation, long userId);

        System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetByCreatedAfterAsync(DateTime date, long userId);
    }
}