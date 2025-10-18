namespace MVCProject.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //Here if i didn't type InvokeAsync or Invoke it will not work and throw this error:  //"InvalidOperationException: No public 'Invoke' or 'InvokeAsync' method found for middleware of type 'MVCProject.Middleware.GlobalExceptionHandlerMiddleware'.
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);

                if(httpContext.Response.StatusCode == 404)
                {
                    await Handle404Async(httpContext);
                }
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                StatusCodes = context.Response.StatusCode,
                Message = "An error occurred while processing your request. from the custom middleware",
                detailed = exception.Message // Remove in production
            };

            return context.Response.WriteAsJsonAsync(response);
        }

        private static Task Handle404Async(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                statusCode = 404,
                message = "The requested resource was not found."
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
