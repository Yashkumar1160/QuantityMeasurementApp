using QuantityMeasurementAppModels.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QuantityMeasurementAppServices.Interfaces
{
    public interface IAuthService
    {
        // Register a new user with email + password
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        // Login an existing user with email + password
        Task<AuthResponse> LoginAsync(LoginRequest request);

        // [ADMIN] Get list of all users
        Task<System.Collections.Generic.List<UserResponse>> GetAllUsersAsync();

        // [ADMIN] Promote a user to Admin role
        Task PromoteToAdminAsync(long userId);
    }
}