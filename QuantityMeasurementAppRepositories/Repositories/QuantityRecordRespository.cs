using System;
using System.Collections.Generic;
using System.Linq;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Context;
using QuantityMeasurementAppRepositories.Interfaces;

namespace QuantityMeasurementAppRepositories.Repositories
{
    public class QuantityRecordRepository : IQuantityRecordRepository
    {
        private readonly AppDbContext context;

        public QuantityRecordRepository(AppDbContext context)
        {
            this.context = context;
        }

        // Method to Save a new record - the entity already has UserId set by the service layer
        public void Save(QuantityMeasurementEntity entity)
        {
            context.QuantityMeasurements.Add(entity);
            context.SaveChanges();
        }

        // Method to Get all records belonging to this user (sorted latest first).
        public List<QuantityMeasurementEntity> GetAll(long userId)
        {
            if (userId == 0)
            {
                return context.QuantityMeasurements
                    .OrderByDescending(e => e.CreatedAt)
                    .ToList();
            }

            return context.QuantityMeasurements
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        // Method to Get records by operation type for this user only
        public List<QuantityMeasurementEntity> GetByOperation(string operation, long userId)
        {
            return context.QuantityMeasurements
                .Where(e => e.Operation == operation && e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        // Method to Get records by measurement type for this user only
        public List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType, long userId)
        {
            return context.QuantityMeasurements
                .Where(e => e.MeasurementType == measurementType && e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        // Method to Get error records for this user only
        public List<QuantityMeasurementEntity> GetErrorHistory(long userId)
        {
            return context.QuantityMeasurements
                .Where(e => e.IsError == true && e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        // Method to Count successful operations of a given type for this user only
        public int GetOperationCount(string operation, long userId)
        {
            return context.QuantityMeasurements
                .Count(e => e.Operation == operation && e.IsError == false && e.UserId == userId);
        }

        // Method to Get records created after a given date for this user only
        public List<QuantityMeasurementEntity> GetByCreatedAfter(DateTime date, long userId)
        {
            return context.QuantityMeasurements
                .Where(e => e.CreatedAt > date && e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }
    }
}