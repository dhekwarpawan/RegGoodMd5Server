using System.Net;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace RegGoodMd5Server.Middelware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next,ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // pass request to next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = context.Response;

            response.StatusCode = ex switch
            {
                ApplicationException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(new
            {
                success = false,
                message = ex.Message,
                statusCode = response.StatusCode
            });

            return context.Response.WriteAsync(result);
        }
    }
}
