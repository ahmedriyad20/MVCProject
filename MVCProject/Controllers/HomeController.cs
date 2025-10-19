using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCProject.Models;

namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            //    StatusCode = HttpContext.Response.StatusCode.ToString(),
            //    Title = HttpContext.Response.Headers.ToString(),
            //     Message = HttpContext.Response.Body.ToString()});

            var statusCode = HttpContext.Response.StatusCode;

            // Get exception details if available
            var exceptionFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
            var statusCodeFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IStatusCodeReExecuteFeature>();

            string title = "An error occurred";
            string message = "An unexpected error occurred while processing your request.";

            // Customize based on status code
            if (statusCode == 404 || statusCodeFeature?.OriginalStatusCode == 404)
            {
                statusCode = 404;
                title = "Page Not Found";
                message = "The page you are looking for could not be found.";
            }
            else if (statusCode == 403)
            {
                title = "Access Denied";
                message = "You do not have permission to access this resource.";
            }
            else if (statusCode >= 500)
            {
                title = "Server Error";
                message = "A server error occurred. Please try again later.";
            }

            // Include exception message in development environment
            if (exceptionFeature?.Error != null && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                message = exceptionFeature.Error.Message;
            }

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode.ToString(),
                Title = title,
                Message = message
            });
        }
    }
}
