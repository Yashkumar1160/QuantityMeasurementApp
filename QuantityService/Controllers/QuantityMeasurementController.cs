using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppServices.Interfaces;

namespace QuantityService.Controllers
{
    // Copied from UC-18 QuantityMeasurementController
    // Only changes: namespace + removed the 5 history/count GET endpoints (moved to HistoryService)
    [ApiController]
    [Route("api/v1/quantities")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityWebService service;

        public QuantityMeasurementController(IQuantityWebService service)
        {
            this.service = service;
        }

        private long GetCurrentUserId()
        {
            string? userIdClaim = User.FindFirstValue("userId");
            if (userIdClaim == null) return 0; // Guest User
            return long.Parse(userIdClaim);
        }

        private string GetCurrentUserEmail()
        {
            return User.FindFirstValue(JwtRegisteredClaimNames.Email) ?? "unknown";
        }

        [HttpPost("compare")]
        public IActionResult Compare([FromBody] QuantityInputRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Compare(request, GetCurrentUserId());
            return Ok(result);
        }

        [HttpPost("convert")]
        public IActionResult Convert([FromBody] ConvertRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Convert(request, GetCurrentUserId());
            return Ok(result);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] ArithmeticRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Add(request, GetCurrentUserId());
            return Ok(result);
        }

        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] ArithmeticRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Subtract(request, GetCurrentUserId());
            return Ok(result);
        }

        [HttpPost("divide")]
        public IActionResult Divide([FromBody] QuantityInputRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Divide(request, GetCurrentUserId());
            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult WhoAmI()
        {
            return Ok(new
            {
                UserId = GetCurrentUserId(),
                Email  = GetCurrentUserEmail(),
                Name   = User.FindFirstValue("name") ?? "unknown",
                Role   = User.FindFirstValue("role") ?? "User"
            });
        }

        // REMOVED from here (now in HistoryService):
        // GET history/operation/{operation}
        // GET history/type/{type}
        // GET history/errored
        // GET history/all
        // GET count/{operation}
    }
}