using System.Diagnostics;

namespace MVCProject.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        // Constructor: ASP.NET Core injects _next and _logger
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // This method is called for EVERY request
        public async Task InvokeAsync(HttpContext context)
        {
            // ===== BEFORE the request goes to the controller =====

            // Create a stopwatch to measure how long the request takes
            var stopwatch = Stopwatch.StartNew();

            // Log the incoming request details
            _logger.LogInformation(
                "Incoming Request: {Method} {Path} from {IpAddress}",
                context.Request.Method,      // GET, POST, PUT, DELETE, etc.
                context.Request.Path,        // /Home/Index, /api/products, etc.
                context.Connection.RemoteIpAddress  // User's IP address
            );

            // Pass the request to the next middleware/controller
            await _next(context);

            // ===== AFTER the controller/next middleware finishes =====

            stopwatch.Stop();

            // Log the response details
            _logger.LogInformation(
                "Completed Request: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,     // 200, 404, 500, etc.
                stopwatch.ElapsedMilliseconds    // How long it took
            );
        }
    }
}
