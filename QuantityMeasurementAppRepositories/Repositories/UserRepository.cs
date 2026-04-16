using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;

namespace QuantityMeasurementAppRepositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext context;

        public UserRepository(DbContext context)
        {
            this.context = context;
        }

        public async System.Threading.Tasks.Task<UserEntity?> FindByEmailAsync(string email)
        {
            return await context.Set<UserEntity>().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async System.Threading.Tasks.Task<UserEntity?> FindByIdAsync(long id)
        {
            return await context.Set<UserEntity>().FindAsync(id);
        }

        public async System.Threading.Tasks.Task<UserEntity> SaveAsync(UserEntity user)
        {
            context.Set<UserEntity>().Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async System.Threading.Tasks.Task UpdateAsync(UserEntity user)
        {
            context.Set<UserEntity>().Update(user);
            await context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<UserEntity>> GetAllAsync()
        {
            var users = await context.Set<UserEntity>().ToListAsync();
            users.Sort((a, b) => DateTime.Compare(a.CreatedAt, b.CreatedAt));
            return users;
        }
    }
}