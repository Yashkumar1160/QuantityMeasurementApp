using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppRepositories.Interfaces;

namespace HistoryService.Controllers
{
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IQuantityRecordRepository repository;

        public HistoryController(IQuantityRecordRepository repository)
        {
            this.repository = repository;
        }

        private long GetCurrentUserId()
        {
            string? userIdClaim = User.FindFirstValue("userId");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token. Please login again.");
            return long.Parse(userIdClaim);
        }

        // ── INTERNAL — called by QuantityService to save a record (no JWT needed) ──
        private const string InternalSecret = "QuantityMeasurement_InternalSecret_2026!";

        [HttpPost("api/v1/internal/history")]
        public async System.Threading.Tasks.Task<IActionResult> SaveRecord([FromBody] QuantityMeasurementEntity entity)
        {
            if (!Request.Headers.TryGetValue("X-Internal-Secret", out var secret) || secret != InternalSecret)
                return Unauthorized(new { Message = "Internal access only. Secret key required." });

            try 
            {
                await repository.SaveAsync(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HistoryService Error] Failed to save record: {ex.Message}");
                if (ex.InnerException != null) 
                    Console.WriteLine($"[HistoryService Inner Error] {ex.InnerException.Message}");
                
                return StatusCode(500, new { Error = "Database Error", Details = ex.Message });
            }
        }

        // Called by AdminService to get ALL records from all users
        [HttpGet("api/v1/internal/records")]
        public async System.Threading.Tasks.Task<IActionResult> GetAllRecordsInternal()
        {
            if (!Request.Headers.TryGetValue("X-Internal-Secret", out var secret) || secret != InternalSecret)
                return Unauthorized(new { Message = "Internal access only. Secret key required." });

            List<QuantityMeasurementEntity> records = await repository.GetAllAsync(0);
            return Ok(QuantityMeasurementResponseDTO.FromEntityList(records));
        }

        // ── EXTERNAL — called by Angular via Gateway (JWT required) ──

        [HttpGet("api/v1/history/all")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> GetAllHistory()
        {
            List<QuantityMeasurementResponseDTO> result = QuantityMeasurementResponseDTO.FromEntityList(
                await repository.GetAllAsync(GetCurrentUserId()));
            return Ok(result);
        }

        [HttpGet("api/v1/history/operation/{operation}")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> GetHistoryByOperation(string operation)
        {
            List<QuantityMeasurementResponseDTO> result = QuantityMeasurementResponseDTO.FromEntityList(
                await repository.GetByOperationAsync(operation, GetCurrentUserId()));
            return Ok(result);
        }

        [HttpGet("api/v1/history/type/{type}")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> GetHistoryByType(string type)
        {
            List<QuantityMeasurementResponseDTO> result = QuantityMeasurementResponseDTO.FromEntityList(
                await repository.GetByMeasurementTypeAsync(type, GetCurrentUserId()));
            return Ok(result);
        }

        [HttpGet("api/v1/history/errored")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> GetErrorHistory()
        {
            List<QuantityMeasurementResponseDTO> result = QuantityMeasurementResponseDTO.FromEntityList(
                await repository.GetErrorHistoryAsync(GetCurrentUserId()));
            return Ok(result);
        }

        [HttpGet("api/v1/history/count/{operation}")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> GetOperationCount(string operation)
        {
            int count = await repository.GetOperationCountAsync(operation, GetCurrentUserId());
            return Ok(count);
        }
    }
}