using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuantityMeasurementAppBusiness.Exceptions;
using QuantityMeasurementAppModels.DTOs;

namespace QuantityMeasurementWebApi.Middleware
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Decide status code based on exception type
            int status = GetStatusCode(context.Exception);
            string error = GetErrorMessage(status);

            // Build the error response
            ErrorResponseDTO response = new ErrorResponseDTO
            {
                Timestamp = DateTime.UtcNow.ToString(),
                Status = status,
                Error = error,
                Message = context.Exception.Message,
                Path = context.HttpContext.Request.Path.ToString()
            };

            // Return the response with correct status code
            context.Result = new ObjectResult(response)
            {
                StatusCode = status
            };

            // Tell ASP.NET Core the exception has been handled
            context.ExceptionHandled = true;
        }

        // Returns 400 for known exceptions, 500 for everything else
        private int GetStatusCode(Exception exception)
        {
            if (exception is QuantityMeasurementException)
            {
                return 400;
            }

            if (exception is ArgumentException)
            {
                return 400;
            }

            if (exception is NotSupportedException)
            {
                return 400;
            }

            return 500;
        }

        // Returns error text based on status code
        private string GetErrorMessage(int status)
        {
            if (status == 400)
            {
                return "Bad Request";
            }

            return "Internal Server Error";
        }
    }
}