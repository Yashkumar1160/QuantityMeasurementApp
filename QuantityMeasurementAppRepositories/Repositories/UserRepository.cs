using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Context;
using QuantityMeasurementAppRepositories.Interfaces;

namespace QuantityMeasurementAppRepositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        // Constructor
        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        // Method to Find user by email - used during login and to check if email already registered
        public UserEntity? FindByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email == email);
        }

        // Method to Find user by their database ID - used to verify JWT claims
        public UserEntity? FindById(long id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        // Method to Save a brand new user to the database
        public UserEntity Save(UserEntity user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        // Method to Update an existing user 
        public void Update(UserEntity user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }
    }
}