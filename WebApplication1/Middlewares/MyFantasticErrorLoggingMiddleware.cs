using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebApiExample.Middlewares
{
    public class MyFantasticErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public MyFantasticErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //Adding value to header
            //httpContext.Response.Headers.Add("sth", "s1234");
            try
            {
                await _next(httpContext);
            }
            catch (Exception exc)
            {
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("Unexpected problem!");
                using var sw = new StreamWriter("Data/log.txt", true);
                await sw.WriteAsync(exc.ToString());
            }
        }

    }
}
