using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppServices.Interfaces;

namespace QuantityMeasurementAppServices.Services
{
    // Handles user registration and login
    public class AuthService : IAuthService
    {
        // user repo
        private readonly IUserRepository userRepository;

        // jwt service
        private readonly JwtService jwtService;

        // Constructor
        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            this.userRepository = userRepository;
            this.jwtService     = jwtService;
        }

        // Method to Register new user
        public AuthResponse Register(RegisterRequest request)
        {
            // Reject if email already exists
            UserEntity existing = userRepository.FindByEmail(request.Email);
            if (existing != null)
                throw new InvalidOperationException("This email is already registered. Please login instead.");

            // Hash password before saving
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Build user entity
            UserEntity newUser = new UserEntity
            {
                Name         = request.Name,
                Email        = request.Email,
                PasswordHash = passwordHash,
                CreatedAt    = DateTime.UtcNow,
                LastLoginAt  = DateTime.UtcNow
            };

            userRepository.Save(newUser);

            // Issue token and return
            string token = jwtService.GenerateToken(newUser);
            return BuildResponse(newUser, token);
        }

        // Method to Login existing user
        public AuthResponse Login(LoginRequest request)
        {
            // Check email exists
            UserEntity user = userRepository.FindByEmail(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // Verify password
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!passwordMatch)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            userRepository.Update(user);

            // Issue token and return
            string token = jwtService.GenerateToken(user);
            return BuildResponse(user, token);
        }

        // Method to Build auth response object
        private AuthResponse BuildResponse(UserEntity user, string token)
        {
            return new AuthResponse
            {
                Token     = token,
                TokenType = "Bearer",
                ExpiresIn = jwtService.GetExpirySeconds(),
                UserId    = user.Id,
                Email     = user.Email,
                Name      = user.Name,
                IssuedAt  = DateTime.UtcNow.ToString("o")
            };
        }
    }
}