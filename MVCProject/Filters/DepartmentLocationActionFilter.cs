using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVCProject.Filters
{
    public class DepartmentLocationActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string? location = null;

            // The parameter name in your action is "department", not "Location"
            if (context.ActionArguments.ContainsKey("department"))
            {
                var department = context.ActionArguments["department"] as DepartmentBranch;
                location = department?.Location;
            }

            if (location != "Egypt" && location != "USA")
            {
                var controller = context.Controller as Controller;

                //controller.ViewBag.Message = "Department location must be either 'Egypt' or 'USA'.";

                controller.ModelState.AddModelError("Location", "Department location must be either 'Egypt' or 'USA'.");

                context.Result = controller.View("AddV2");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

       
    }
}
