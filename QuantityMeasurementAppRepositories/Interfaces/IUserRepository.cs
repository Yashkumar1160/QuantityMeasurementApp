using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppRepositories.Interfaces
{
    public interface IUserRepository
    {
        // Find user by email - used during login and register 
        System.Threading.Tasks.Task<UserEntity?> FindByEmailAsync(string email);

        // Find user by internal database ID - used to verify JWT claims
        System.Threading.Tasks.Task<UserEntity?> FindByIdAsync(long id);

        // Save a new user to the database
        System.Threading.Tasks.Task<UserEntity> SaveAsync(UserEntity user);

        // Update an existing user 
        System.Threading.Tasks.Task UpdateAsync(UserEntity user);

        System.Threading.Tasks.Task<System.Collections.Generic.List<UserEntity>> GetAllAsync();
    }
}