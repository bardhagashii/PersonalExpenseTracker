namespace PersonalExpenseTracker.Exceptions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);  // proceed to the next middleware
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;  // internal Server Error
                context.Response.ContentType = "application/json";

                // return an error message as JSON
                var errorResponse = new
                {
                    Message = "An unexpected error occurred.",
                    Detail = ex.Message
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
