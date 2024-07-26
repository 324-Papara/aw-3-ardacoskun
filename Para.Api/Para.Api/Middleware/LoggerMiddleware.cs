using System.Text;

namespace Para.Api.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerMiddleware> _logger;

        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);

            var originalResponseBodyStream = context.Response.Body;
            using var newResponseBodyStream = new MemoryStream();
            context.Response.Body = newResponseBodyStream;

            await _next(context);

            await LogResponse(context);

            newResponseBodyStream.Seek(0, SeekOrigin.Begin);
            await newResponseBodyStream.CopyToAsync(originalResponseBodyStream);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var requestBody = await ReadRequestBody(context.Request);
            context.Request.Body.Position = 0;

            _logger.LogInformation("Request: {Method} {Url}", context.Request.Method, context.Request.Path);
            _logger.LogDebug("Request Body: {Body}", requestBody);
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            return await reader.ReadToEndAsync();
        }

        private async Task LogResponse(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation("Response Status Code: {StatusCode}", context.Response.StatusCode);
            _logger.LogDebug("Response Body: {ResponseBody}", responseBody);
        }
    }
}
