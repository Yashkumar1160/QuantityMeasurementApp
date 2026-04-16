using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppServices.Interfaces;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        // Auth Service instance
        private readonly IAuthService authService;

        // Constructor
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Register a new account with email and password
        /// </summary>
        /// <remarks>
        /// Example request:
        ///
        ///     POST /api/v1/auth/register
        ///     {
        ///         "name": "Yash Kumar",
        ///         "email": "yash@gmail.com",
        ///         "password": "mypassword123"
        ///     }
        ///
        /// Returns a JWT token. Use it in the Authorization header for all other endpoints:
        ///     Authorization: Bearer {token}
        /// </remarks>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        public async System.Threading.Tasks.Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            AuthResponse response = await authService.RegisterAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Login with your email and password
        /// </summary>
        /// <remarks>
        /// Example request:
        ///
        ///     POST /api/v1/auth/login
        ///     {
        ///         "email": "yash@gmail.com",
        ///         "password": "mypassword123"
        ///     }
        ///
        /// Returns a JWT token. Use it in the Authorization header for all other endpoints:
        ///     Authorization: Bearer {token}
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async System.Threading.Tasks.Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            AuthResponse response = await authService.LoginAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Test that auth controller is running
        /// </summary>
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { Message = "Auth running. Register at POST /api/v1/auth/register, Login at POST /api/v1/auth/login" });
        }
    }
}