using QuantityMeasurementAppModels.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QuantityMeasurementAppRepositories.Interfaces
{
    public interface IUserRepository
    {
        // Find user by email - used during login and register 
        Task<UserEntity?> FindByEmailAsync(string email);

        // Find user by internal database ID - used to verify JWT claims
        Task<UserEntity?> FindByIdAsync(long id);

        // Save a new user to the database
        Task<UserEntity> SaveAsync(UserEntity user);

        // Update an existing user 
        Task UpdateAsync(UserEntity user);

        Task<System.Collections.Generic.List<UserEntity>> GetAllAsync();
    }
}