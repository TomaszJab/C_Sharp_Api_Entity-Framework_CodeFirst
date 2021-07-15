using Microsoft.AspNetCore.Builder;

namespace WebApiExample.Middlewares
{
    public static class FantasticMiddlewareExtensions
    {
        public static IApplicationBuilder UseGreatErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyFantasticErrorLoggingMiddleware>();
        }
    }
}
