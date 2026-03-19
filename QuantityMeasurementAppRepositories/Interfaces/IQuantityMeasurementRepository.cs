using System.Collections.Generic;
using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppRepositories.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        // UC15 original methods
        void Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAll();

        // UC16 methods
        List<QuantityMeasurementEntity> GetByOperation(string operation);
        List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType);
        int GetTotalCount();
        void DeleteAll();
        string GetPoolStatistics();
        void ReleaseResources();
    }
}   