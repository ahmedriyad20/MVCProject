using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Ensure this is present
using System;

namespace MVCProject.Filters
{
    public class EditInstructorResultFilter : Attribute, IResultFilter
    {
        private readonly ILogger<EditInstructorResultFilter> _logger;

        public void OnResultExecuted(ResultExecutedContext context)
        {


            //var user = context.HttpContext.User.Identity.Name;
            var userIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var instructorId = context.RouteData.Values["id"];

            Console.WriteLine($"Instructor {instructorId} was edited by {userIp} at {DateTime.Now}");
            //_logger.LogInformation($"Instructor {instructorId} was edited by {user} at {DateTime.Now}");

            if (context.Controller is Controller controller)
            {
                controller.ViewBag.LastEdited = DateTime.Now.ToString("f");
                controller.ViewBag.userIp = userIp;
                controller.ViewBag.Message = "✅ Instructor details updated successfully.";
            }
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            

            //var user = context.HttpContext.User.Identity.Name;
            var userIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var instructorId = context.RouteData.Values["id"];

            Console.WriteLine($"Instructor {instructorId} was edited by {userIp} at {DateTime.Now}");
            //_logger.LogInformation($"Instructor {instructorId} was edited by {user} at {DateTime.Now}");

            if (context.Controller is Controller controller)
            {
                controller.ViewBag.LastEdited = DateTime.Now.ToString("f");
                controller.ViewBag.userIp = userIp;
                controller.ViewBag.Message = "✅ Instructor details updated successfully.";
            }
        }
    }
}
