using System.Net;

namespace SMSMicroService.Infrastructures
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                HandleExceptionAsync(context, e);
            }
        }

        private static void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.Response.WriteAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString() ?? string.Empty);
        }
    }
}
