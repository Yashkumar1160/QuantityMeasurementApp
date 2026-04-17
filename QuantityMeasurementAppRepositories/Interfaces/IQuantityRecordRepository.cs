using System;
using System.Collections.Generic;
using QuantityMeasurementAppModels.Entities;
using System.Threading.Tasks;

namespace QuantityMeasurementAppRepositories.Interfaces
{
    public interface IQuantityRecordRepository
    {
        Task SaveAsync(QuantityMeasurementEntity entity);

        Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetAllAsync(long userId);

        Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetByOperationAsync(string operation, long userId);

        Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetByMeasurementTypeAsync(string measurementType, long userId);

        Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetErrorHistoryAsync(long userId);

        Task<int> GetOperationCountAsync(string operation, long userId);

        Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetByCreatedAfterAsync(DateTime date, long userId);
    }
}