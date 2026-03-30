using QuantityMeasurementAppModels.DTOs;

namespace QuantityMeasurementAppServices.Interfaces
{
    public interface IAuthService
    {
        // Register a new user with email + password
        AuthResponse Register(RegisterRequest request);

        // Login an existing user with email + password
        AuthResponse Login(LoginRequest request);
    }
}