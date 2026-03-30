using QuantityMeasurementAppModels.Entities;

namespace QuantityMeasurementAppRepositories.Interfaces
{
    public interface IUserRepository
    {
        // Find user by email - used during login and register 
        UserEntity? FindByEmail(string email);

        // Find user by internal database ID - used to verify JWT claims
        UserEntity? FindById(long id);

        // Save a new user to the database
        UserEntity Save(UserEntity user);

        // Update an existing user 
        void Update(UserEntity user);
    }
}