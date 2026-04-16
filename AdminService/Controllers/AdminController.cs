using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers
{
    
    [ApiController]
    [Route("api/v1/admin")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly HttpClient authClient;
        private readonly HttpClient historyClient;

        public AdminController(IHttpClientFactory factory)
        {
            authClient    = factory.CreateClient("AuthService");
            historyClient = factory.CreateClient("HistoryService");
        }

        private bool IsAdmin()
        {
            string? role = User.FindFirstValue("role");
            return role == "Admin";
        }

        // GET /api/v1/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!IsAdmin()) return Forbid();

            var response = await authClient.GetAsync("api/v1/internal/users");
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }

        // GET /api/v1/admin/records
        [HttpGet("records")]
        public async Task<IActionResult> GetAllRecords()
        {
            if (!IsAdmin()) return Forbid();

            var response = await historyClient.GetAsync("api/v1/internal/records");
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }

        // POST /api/v1/admin/promote/{userId}
        [HttpPost("promote/{userId}")]
        public async Task<IActionResult> PromoteUser(long userId)
        {
            if (!IsAdmin()) return Forbid();

            var response = await authClient.PostAsync($"api/v1/internal/promote/{userId}", null);
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }
    }
}