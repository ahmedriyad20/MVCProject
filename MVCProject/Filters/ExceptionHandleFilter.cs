using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MVCProject.Models;
using System.Diagnostics;

namespace MVCProject.Filters
{
    public class ExceptionHandleFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {


            // Create error view model with exception details
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
                StatusCode = "500",
                Title = "An Error Occurred",
                Message = context.Exception.Message
            };

            // Create ViewResult with the model
            var viewResult = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = errorViewModel
                }
            };

            context.Result = viewResult;

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 500;

        }
    }
}
