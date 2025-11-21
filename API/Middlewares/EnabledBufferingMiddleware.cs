
namespace Sindika.AspNet.app015.API.Middlewares
{
    public class EnabledBufferingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<EnabledBufferingMiddleware> _logger;
        public EnabledBufferingMiddleware(RequestDelegate next, ILogger<EnabledBufferingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                await _next(context);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
