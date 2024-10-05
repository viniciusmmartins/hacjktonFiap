using Hackaton.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Hackaton.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandlerExceptionAsync(context, ex);
            }
        }

        private static Task HandlerExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            switch (ex)
            {
                case ArgumentException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case ConflictException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    break;

                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
            }

            context.Response.StatusCode = statusCode;

            context.Response.ContentType = "application/json";

            var response = new
            {
                error = ex.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
