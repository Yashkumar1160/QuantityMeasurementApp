using QuantityMeasurementAppModels.DTOs;

namespace QuantityMeasurementAppServices.Interfaces
{
    public interface IAuthService
    {
        // Register a new user with email + password
        System.Threading.Tasks.Task<AuthResponse> RegisterAsync(RegisterRequest request);

        // Login an existing user with email + password
        System.Threading.Tasks.Task<AuthResponse> LoginAsync(LoginRequest request);

        // [ADMIN] Get list of all users
        System.Threading.Tasks.Task<System.Collections.Generic.List<UserResponse>> GetAllUsersAsync();

        // [ADMIN] Promote a user to Admin role
        System.Threading.Tasks.Task PromoteToAdminAsync(long userId);
    }
}