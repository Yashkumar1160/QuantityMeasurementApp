using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuantityMeasurementAppModels.DTOs;
using QuantityMeasurementAppModels.Entities;
using QuantityMeasurementAppServices.Interfaces;

namespace QuantityMeasurementAppServices.Services
{
    public class QuantityWebServiceImpl : IQuantityWebService
    {
        private readonly IQuantityMeasurementService service;
        private readonly HttpClient historyClient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<QuantityWebServiceImpl> logger;

        // Shared secret to secure internal calls to HistoryService
        private const string InternalSecret = "QuantityMeasurement_InternalSecret_2026!";

        public QuantityWebServiceImpl(
            IQuantityMeasurementService service, 
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            ILogger<QuantityWebServiceImpl> logger)
        {
            this.service = service;
            this.historyClient = httpClientFactory.CreateClient("HistoryService");
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        private void SaveToHistoryAsync(QuantityMeasurementEntity entity)
        {
            // GUEST CHECK: Do not save history for unauthenticated users (UserId = 0)
            if (entity.UserId <= 0) return;

            // We do this in a "fire and forget" task to keep the UI fast, 
            // but we add tracing and security headers first.
            Task.Run(async () =>
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/internal/history");
                    request.Content = JsonContent.Create(entity);

                    // 1. ADD SECURITY: Prove this is an internal call from another service
                    request.Headers.Add("X-Internal-Secret", InternalSecret);

                    // 2. ADD TRACING: Pass the unique request ID (Correlation ID)
                    var correlationId = httpContextAccessor.HttpContext?.Items["X-Correlation-ID"]?.ToString();
                    if (!string.IsNullOrEmpty(correlationId))
                    {
                        request.Headers.Add("X-Correlation-ID", correlationId);
                    }

                    // 3. SEND (with a simple retry if it fails)
                    int retryCount = 0;
                    while (retryCount < 3)
                    {
                        var response = await historyClient.SendAsync(request);
                        if (response.IsSuccessStatusCode) break;
                        
                        retryCount++;
                        logger.LogWarning($"Attempt {retryCount} to save history failed. Retrying...");
                        await Task.Delay(500); // Wait a bit before retrying
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to save record to HistoryService after multiple attempts.");
                }
            });
        }

        public QuantityMeasurementResponseDTO Compare(QuantityInputRequest request, long userId)
        {
            bool result = service.Compare(request.ThisQuantityDTO, request.ThatQuantityDTO);

            var dto = new QuantityMeasurementResponseDTO
            {
                ThisValue           = request.ThisQuantityDTO.Value,
                ThisUnit            = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType = request.ThisQuantityDTO.MeasurementType,
                ThatValue           = request.ThatQuantityDTO.Value,
                ThatUnit            = request.ThatQuantityDTO.UnitName,
                ThatMeasurementType = request.ThatQuantityDTO.MeasurementType,
                Operation           = "Compare",
                ResultString        = result.ToString(),
                IsError             = false
            };

            var entity = new QuantityMeasurementEntity(
                userId, "Compare",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                request.ThatQuantityDTO.Value, request.ThatQuantityDTO.UnitName,
                result ? 1 : 0,
                request.ThisQuantityDTO.MeasurementType);

            SaveToHistoryAsync(entity);
            return dto;
        }

        public QuantityMeasurementResponseDTO Convert(ConvertRequest request, long userId)
        {
            QuantityDTO result = service.Convert(request.ThisQuantityDTO, request.TargetUnit);

            var dto = new QuantityMeasurementResponseDTO
            {
                ThisValue           = request.ThisQuantityDTO.Value,
                ThisUnit            = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType = request.ThisQuantityDTO.MeasurementType,
                Operation           = "Convert",
                ResultValue         = result.Value,
                ResultUnit          = request.TargetUnit,
                IsError             = false
            };

            var entity = new QuantityMeasurementEntity(
                userId, "Convert",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                result.Value,
                request.ThisQuantityDTO.MeasurementType);

            SaveToHistoryAsync(entity);
            return dto;
        }

        public QuantityMeasurementResponseDTO Add(ArithmeticRequest request, long userId)
        {
            QuantityDTO result = service.Add(request.ThisQuantityDTO, request.ThatQuantityDTO, request.TargetUnit);

            var dto = new QuantityMeasurementResponseDTO
            {
                ThisValue             = request.ThisQuantityDTO.Value,
                ThisUnit              = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType   = request.ThisQuantityDTO.MeasurementType,
                ThatValue             = request.ThatQuantityDTO.Value,
                ThatUnit              = request.ThatQuantityDTO.UnitName,
                ThatMeasurementType   = request.ThatQuantityDTO.MeasurementType,
                Operation             = "Add",
                ResultValue           = result.Value,
                ResultUnit            = request.TargetUnit,
                ResultMeasurementType = request.ThisQuantityDTO.MeasurementType,
                IsError               = false
            };

            var entity = new QuantityMeasurementEntity(
                userId, "Add",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                request.ThatQuantityDTO.Value, request.ThatQuantityDTO.UnitName,
                result.Value,
                request.ThisQuantityDTO.MeasurementType);

            SaveToHistoryAsync(entity);
            return dto;
        }

        public QuantityMeasurementResponseDTO Subtract(ArithmeticRequest request, long userId)
        {
            QuantityDTO result = service.Subtract(request.ThisQuantityDTO, request.ThatQuantityDTO, request.TargetUnit);

            var dto = new QuantityMeasurementResponseDTO
            {
                ThisValue             = request.ThisQuantityDTO.Value,
                ThisUnit              = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType   = request.ThisQuantityDTO.MeasurementType,
                ThatValue             = request.ThatQuantityDTO.Value,
                ThatUnit              = request.ThatQuantityDTO.UnitName,
                ThatMeasurementType   = request.ThatQuantityDTO.MeasurementType,
                Operation             = "Subtract",
                ResultValue           = result.Value,
                ResultUnit            = request.TargetUnit,
                ResultMeasurementType = request.ThisQuantityDTO.MeasurementType,
                IsError               = false
            };

            var entity = new QuantityMeasurementEntity(
                userId, "Subtract",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                request.ThatQuantityDTO.Value, request.ThatQuantityDTO.UnitName,
                result.Value,
                request.ThisQuantityDTO.MeasurementType);

            SaveToHistoryAsync(entity);
            return dto;
        }

        public QuantityMeasurementResponseDTO Divide(QuantityInputRequest request, long userId)
        {
            double result = service.Divide(request.ThisQuantityDTO, request.ThatQuantityDTO);

            var dto = new QuantityMeasurementResponseDTO
            {
                ThisValue           = request.ThisQuantityDTO.Value,
                ThisUnit            = request.ThisQuantityDTO.UnitName,
                ThisMeasurementType = request.ThisQuantityDTO.MeasurementType,
                ThatValue           = request.ThatQuantityDTO.Value,
                ThatUnit            = request.ThatQuantityDTO.UnitName,
                ThatMeasurementType = request.ThatQuantityDTO.MeasurementType,
                Operation           = "Divide",
                ResultValue         = result,
                IsError             = false
            };

            var entity = new QuantityMeasurementEntity(
                userId, "Divide",
                request.ThisQuantityDTO.Value, request.ThisQuantityDTO.UnitName,
                request.ThatQuantityDTO.Value, request.ThatQuantityDTO.UnitName,
                result,
                request.ThisQuantityDTO.MeasurementType);

            SaveToHistoryAsync(entity);
            return dto;
        }

        // History methods remain unsupported here as they are now handled by HistoryService
        public List<QuantityMeasurementResponseDTO> GetHistoryByOperation(string op, long uid) => throw new NotSupportedException();
        public List<QuantityMeasurementResponseDTO> GetHistoryByType(string type, long uid) => throw new NotSupportedException();
        public List<QuantityMeasurementResponseDTO> GetErrorHistory(long uid) => throw new NotSupportedException();
        public List<QuantityMeasurementResponseDTO> GetAllHistory(long uid) => throw new NotSupportedException();
        public int GetOperationCount(string op, long uid) => throw new NotSupportedException();
    }
}