using E_CommerceSharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace E_CommerceSharedLibrary.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Handle custom status codes
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    await WriteResponse(context, "Warning", "Too many requests. Please try again later", StatusCodes.Status429TooManyRequests);
                }
                else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await WriteResponse(context, "Alert", "You are not authorized to access", StatusCodes.Status401Unauthorized);
                }
                else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    await WriteResponse(context, "Out of Access", "You are not allowed to access.", StatusCodes.Status403Forbidden);
                }
            }
            catch (Exception ex)
            {
                LogException.logException(ex);

                var statusCode = ex is UnauthorizedAccessException
                    ? StatusCodes.Status408RequestTimeout
                    : StatusCodes.Status500InternalServerError;

                var title = ex is UnauthorizedAccessException ? "Out of Time" : "Error";
                var message = ex is UnauthorizedAccessException
                    ? "Request timeout. Try again."
                    : "Sorry, internal server error. Kindly try again.";

                await WriteResponse(context, title, message, statusCode);
            }
        }

        private static async Task WriteResponse(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Title = title,
                Message = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
