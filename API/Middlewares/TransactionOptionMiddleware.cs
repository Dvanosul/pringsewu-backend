using System.Text.Json;
using Sindika.AspNet.Common.Models;
using Sindika.AspNet.Request;

namespace Sindika.AspNet.app015.API.Middlewares
{
    public class TransactionOptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TransactionOptionMiddleware> _logger;
        public TransactionOptionMiddleware(RequestDelegate next, ILogger<TransactionOptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, TransactionOption transactionOption)
        {
            if (string.IsNullOrWhiteSpace(context.Request.Path))
            {
                await _next(context);
                return;
            }

            transactionOption.RollbackOnFailure = true;

            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                string json = await reader.ReadToEndAsync();

                context.Request.Body.Position = 0;

                if (string.IsNullOrWhiteSpace(json))
                {
                    await _next(context);
                    return;
                }

                try
                {
                    var baseRequest = JsonSerializer.Deserialize<BaseRequest<object>>(json);

                    if (baseRequest?.Options != null)
                    {
                        transactionOption.RollbackOnFailure = baseRequest.Options.RollbackOnFailure;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, $"JSON deserialization error: {ex.Message}");
                }
                catch (Exception)
                {
                    throw;
                }
            }

            await _next(context);
        }
    }
}
