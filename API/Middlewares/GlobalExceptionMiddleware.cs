
using System.Net;
using System.Text.Json;
using Sindika.AspNet.Exceptions.BadRequest;
using Sindika.AspNet.Exceptions.Forbidden;
using Sindika.AspNet.Exceptions.InternalServerError;
using Sindika.AspNet.Exceptions.NotFound;
using Sindika.AspNet.Exceptions.Unauthorized;
using Sindika.AspNet.Response;

namespace Sindika.AspNet.app015.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var code = "ERR-SRV-500";
            var message = "An internal server error occurred. Please try again later.";
            ErrorResponse<object>? error = null;
            var success = false;

            (code, message, statusCode) = HandleExceptionCodeMessage(exception, code, message, statusCode);

            var response = new
            {
                success,
                code,
                message,
                error,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private (string, string, int) HandleExceptionCodeMessage(Exception exception, string code, string message, int statusCode)
        {
            if (exception is UnauthorizedException unauthorizedException)
            {
                code = unauthorizedException.GetCode();
                message = unauthorizedException.GetUserMessage();
                statusCode = (int)unauthorizedException.StatusCode;
            }
            else if (exception is ForbiddenException forbiddenException)
            {
                code = forbiddenException.GetCode();
                message = forbiddenException.GetUserMessage();
                statusCode = (int)forbiddenException.StatusCode;
            }
            else if (exception is BadRequestException badRequestException)
            {
                code = badRequestException.GetCode();
                message = badRequestException.GetUserMessage();
                statusCode = (int)badRequestException.StatusCode;
            }
            else if (exception is NotFoundException notFoundException)
            {
                code = notFoundException.GetCode();
                message = notFoundException.GetUserMessage();
                statusCode = (int)notFoundException.StatusCode;
            }
            else if (exception is InternalServerErrorException internalServerErrorException)
            {
                code = internalServerErrorException.GetCode();
                message = internalServerErrorException.GetUserMessage();
                _logger.LogError(exception, internalServerErrorException.GetMessage());
            }
            else if (exception is TimeoutException timeoutException)
            {
                code = "ERR-OUT-001";
                message = timeoutException.Message;
                statusCode = (int)HttpStatusCode.RequestTimeout;
                _logger.LogError(exception, timeoutException.Message);
            }
            else if (exception is ArgumentNullException argumentNullException)
            {
                code = "ERR-ARG-001";
                message = argumentNullException.Message;
                statusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(exception, argumentNullException.Message);
            }
            else if (exception is InvalidOperationException invalidOperationException)
            {
                code = "ERR-OPR-001";
                message = invalidOperationException.Message;
                statusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(exception, invalidOperationException.Message);
            }
            else
            {
                Console.WriteLine(exception.StackTrace);
                _logger.LogError(exception, exception.Message);
            }

            return (code, message, statusCode);
        }
    }
}
