using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppRepositories.Context;

namespace QuantityMeasurementAppRepositories.Repositories
{
    public class QuantityRecordRepository : IQuantityRecordRepository
    {
        // Instance of AppDbContext
        private readonly AppDbContext context;

        // Constructor 
        public QuantityRecordRepository(AppDbContext context)
        {
            this.context = context;
        }

        // Method to add entity to ef core and save it to database
        public void Save(QuantityMeasurementEntity entity)
        {
            context.QuantityMeasurements.Add(entity);
            context.SaveChanges();
        }

        // Method to get all records (sorts by latest first)
        public List<QuantityMeasurementEntity> GetAll()
        {
            return context.QuantityMeasurements
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        //  Method to get records according to specific operation
        public List<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            return context.QuantityMeasurements
                .Where(e => e.Operation == operation)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }
        
        // Method to get records by measurement type
        public List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType)
        {
            return context.QuantityMeasurements
                .Where(e => e.MeasurementType == measurementType)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        // Method to get records by errors
        public List<QuantityMeasurementEntity> GetErrorHistory()
        {
            return context.QuantityMeasurements
                .Where(e => e.IsError == true)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }


        // Method to get total operations count 
        public int GetOperationCount(string operation)
        {
            return context.QuantityMeasurements
                .Count(e => e.Operation == operation && e.IsError == false);
        }

        // Method to get records after a specific date
        public List<QuantityMeasurementEntity> GetByCreatedAfter(DateTime date)
        {
            return context.QuantityMeasurements
                .Where(e => e.CreatedAt > date)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }
    }
}