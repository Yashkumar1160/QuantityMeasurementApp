using System;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;
using QuantityMeasurementAppServices.Interfaces;

namespace QuantityMeasurementAppServices.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly JwtService jwtService;

        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            this.userRepository = userRepository;
            this.jwtService     = jwtService;
        }

        public async System.Threading.Tasks.Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            UserEntity existing = await userRepository.FindByEmailAsync(request.Email);
            if (existing != null)
                throw new InvalidOperationException("This email is already registered.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            UserEntity newUser = new UserEntity
            {
                Name         = request.Name,
                Email        = request.Email,
                PasswordHash = passwordHash,
                Role         = "User",       // default role
                CreatedAt    = DateTime.UtcNow,
                LastLoginAt  = DateTime.UtcNow
            };

            await userRepository.SaveAsync(newUser);

            string token = jwtService.GenerateToken(newUser);
            return BuildResponse(newUser, token);
        }

        public async System.Threading.Tasks.Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            UserEntity user = await userRepository.FindByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!passwordMatch)
                throw new UnauthorizedAccessException("Invalid email or password.");

            user.LastLoginAt = DateTime.UtcNow;
            await userRepository.UpdateAsync(user);

            string token = jwtService.GenerateToken(user);
            return BuildResponse(user, token);
        }

        public async System.Threading.Tasks.Task<System.Collections.Generic.List<UserResponse>> GetAllUsersAsync()
        {
            var users = await userRepository.GetAllAsync();
            var result = new System.Collections.Generic.List<UserResponse>();
            foreach (var u in users)
            {
                result.Add(new UserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                });
            }
            return result;
        }

        public async System.Threading.Tasks.Task PromoteToAdminAsync(long userId)
        {
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.Role = "Admin";
            await userRepository.UpdateAsync(user);
        }

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
                IssuedAt  = DateTime.UtcNow.ToString("o"),
                Role      = user.Role    // NEW
            };
        }
    }
}