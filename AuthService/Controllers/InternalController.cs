using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementAppRepositories.Interfaces;

namespace AuthService.Controllers
{
    // No [Authorize] — this is called by AdminService internally, not by Angular
    [ApiController]
    [Route("api/v1/internal")]
    public class InternalController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private const string InternalSecret = "QuantityMeasurement_InternalSecret_2026!";

        public InternalController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        // AdminService calls this to get all users
        [HttpGet("users")]
        public async System.Threading.Tasks.Task<IActionResult> GetAllUsers()
        {
            if (!Request.Headers.TryGetValue("X-Internal-Secret", out var secret) || secret != InternalSecret)
                return Unauthorized(new { Message = "Internal access only. Secret key required." });

            var users = await userRepository.GetAllAsync();
            var result = users.Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.Role,
                u.CreatedAt,
                u.LastLoginAt
            });
            return Ok(result);
        }

        // AdminService calls this to promote a user
        [HttpPost("promote/{userId}")]
        public async System.Threading.Tasks.Task<IActionResult> PromoteUser(long userId)
        {
            if (!Request.Headers.TryGetValue("X-Internal-Secret", out var secret) || secret != InternalSecret)
                return Unauthorized(new { Message = "Internal access only. Secret key required." });

            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = "User not found" });

            user.Role = "Admin";
            await userRepository.UpdateAsync(user);
            return Ok(new { Message = user.Name + " is now an Admin" });
        }
    }
}