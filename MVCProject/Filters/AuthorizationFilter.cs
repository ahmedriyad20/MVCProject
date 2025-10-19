using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVCProject.Filters
{
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private bool _isAuthorized;

        public AuthorizationFilter(bool isAuthorized)
        {
            _isAuthorized = isAuthorized;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!_isAuthorized)
            {
                context.Result = new ViewResult()
                {
                    ViewName = "Unauthorized",
                    StatusCode = 403
                };
            }
        }
    }
}
