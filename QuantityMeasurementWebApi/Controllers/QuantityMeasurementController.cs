using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppServices.Interfaces;

namespace QuantityMeasurementAppWebAPI.Controllers
{
    /// <summary>
    /// REST API for quantity measurement operations.
    /// ALL endpoints require a valid JWT token in the Authorization header.
    /// Format: Authorization: Bearer {your_token}
    /// </summary>
    [ApiController]
    [Route("api/v1/quantities")]
    [Authorize]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityWebService service;

        public QuantityMeasurementController(IQuantityWebService service)
        {
            this.service = service;
        }

        // Reads the current user's ID from the JWT token claims.
        // The "userId" claim was set in JwtService.GenerateToken().
        private long GetCurrentUserId()
        {
            string? userIdClaim = User.FindFirstValue("userId");
            if (userIdClaim == null)
                return 0;
            return long.Parse(userIdClaim);
        }

        // Reads the current user's email from the JWT token claims.
        private string GetCurrentUserEmail()
        {
            return User.FindFirstValue(JwtRegisteredClaimNames.Email) ?? "unknown";
        }

        // ==================== POST Endpoints ====================

        /// <summary>
        /// Compare two quantities for equality
        /// </summary>
        /// <remarks>
        /// Requires: Authorization: Bearer {token}
        ///
        /// Example request:
        ///
        ///     POST /api/v1/quantities/compare
        ///     {
        ///         "thisQuantityDTO": { "value": 1.0, "unitName": "Feet", "measurementType": "Length" },
        ///         "thatQuantityDTO": { "value": 12.0, "unitName": "Inch", "measurementType": "Length" }
        ///     }
        ///
        /// </remarks>
        [HttpPost("compare")]
        [ProducesResponseType(typeof(QuantityMeasurementResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Compare([FromBody] QuantityInputRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Compare(request, GetCurrentUserId());
            return Ok(result);
        }

        /// <summary>
        /// Convert a quantity to a different unit
        /// </summary>
        [HttpPost("convert")]
        [ProducesResponseType(typeof(QuantityMeasurementResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Convert([FromBody] ConvertRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Convert(request, GetCurrentUserId());
            return Ok(result);
        }

        /// <summary>
        /// Add two quantities together
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(typeof(QuantityMeasurementResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Add([FromBody] ArithmeticRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Add(request, GetCurrentUserId());
            return Ok(result);
        }

        /// <summary>
        /// Subtract one quantity from another
        /// </summary>
        [HttpPost("subtract")]
        [ProducesResponseType(typeof(QuantityMeasurementResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Subtract([FromBody] ArithmeticRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Subtract(request, GetCurrentUserId());
            return Ok(result);
        }

        /// <summary>
        /// Divide one quantity by another
        /// </summary>
        [HttpPost("divide")]
        [ProducesResponseType(typeof(QuantityMeasurementResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Divide([FromBody] QuantityInputRequest request)
        {
            QuantityMeasurementResponseDTO result = service.Divide(request, GetCurrentUserId());
            return Ok(result);
        }

        // ==================== GET Endpoints ====================

        /// <summary>
        /// Get operation history filtered by operation type (only your own records)
        /// </summary>
        [HttpGet("history/operation/{operation}")]
        [ProducesResponseType(typeof(List<QuantityMeasurementResponseDTO>), 200)]
        [ProducesResponseType(401)]
        public IActionResult GetHistoryByOperation(string operation)
        {
            List<QuantityMeasurementResponseDTO> result = service.GetHistoryByOperation(operation, GetCurrentUserId());
            return Ok(result);
        }

        /// <summary>
        /// Get history filtered by measurement type (only your own records)
        /// </summary>
        [HttpGet("history/type/{type}")]
        [ProducesResponseType(typeof(List<QuantityMeasurementResponseDTO>), 200)]
        [ProducesResponseType(401)]
        public IActionResult GetHistoryByType(string type)
        {
            List<QuantityMeasurementResponseDTO> result = service.GetHistoryByType(type, GetCurrentUserId());
            return Ok(result);
        }

        /// <summary>
        /// Get all records that resulted in an error (only your own records)
        /// </summary>
        [HttpGet("history/errored")]
        [ProducesResponseType(typeof(List<QuantityMeasurementResponseDTO>), 200)]
        [ProducesResponseType(401)]
        public IActionResult GetErrorHistory()
        {
            List<QuantityMeasurementResponseDTO> result = service.GetErrorHistory(GetCurrentUserId());
            return Ok(result);
        }

        /// <summary>
        /// Get count of successful operations by operation type (only your own records)
        /// </summary>
        [HttpGet("count/{operation}")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public IActionResult GetOperationCount(string operation)
        {
            int count = service.GetOperationCount(operation, GetCurrentUserId());
            return Ok(count);
        }

        /// <summary>
        /// Get info about the currently logged-in user (extracted from JWT claims)
        /// </summary>
        [HttpGet("me")]
        public IActionResult WhoAmI()
        {
            return Ok(new
            {
                UserId = GetCurrentUserId(),
                Email = GetCurrentUserEmail(),
                Name = User.FindFirstValue("name") ?? "unknown"
            });
        }
    }
}