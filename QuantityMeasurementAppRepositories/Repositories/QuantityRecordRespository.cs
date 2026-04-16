using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;

namespace QuantityMeasurementAppRepositories.Repositories
{
    public class QuantityRecordRepository : IQuantityRecordRepository
    {
        private readonly DbContext context;

        public QuantityRecordRepository(DbContext context)
        {
            this.context = context;
        }

        public async System.Threading.Tasks.Task SaveAsync(QuantityMeasurementEntity entity)
        {
            context.Set<QuantityMeasurementEntity>().Add(entity);
            await context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetAllAsync(long userId)
        {
            return await context.Set<QuantityMeasurementEntity>()
                .Where(e => userId == 0 || e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetByOperationAsync(string operation, long userId)
        {
            return await context.Set<QuantityMeasurementEntity>()
                .Where(e => e.Operation == operation && e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetByMeasurementTypeAsync(string measurementType, long userId)
        {
            return await context.Set<QuantityMeasurementEntity>()
                .Where(e => e.MeasurementType == measurementType && e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetErrorHistoryAsync(long userId)
        {
            return await context.Set<QuantityMeasurementEntity>()
                .Where(e => e.IsError == true && e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<int> GetOperationCountAsync(string operation, long userId)
        {
            return await context.Set<QuantityMeasurementEntity>()
                .Where(e => e.Operation == operation && e.IsError == false && e.UserId == userId)
                .CountAsync();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<QuantityMeasurementEntity>> GetByCreatedAfterAsync(DateTime date, long userId)
        {
            return await context.Set<QuantityMeasurementEntity>()
                .Where(e => e.CreatedAt > date && e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }
    }
}