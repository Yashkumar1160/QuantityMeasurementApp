using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppRepositories.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAll();
    }
}