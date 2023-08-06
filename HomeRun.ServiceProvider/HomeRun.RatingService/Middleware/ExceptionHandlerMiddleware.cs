using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace HomeRun.RatingService.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger) 
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("Something Went Wrong  ==> {ex}",ex);
               
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; 
                ProblemDetails problem = new()
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Type = "Bad Request",
                        Title = "Error Has Occured",
                        Detail = ex.Message ?? "Error Has Occured"
                    };

                string json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
                    
            }
        }
    }
}
