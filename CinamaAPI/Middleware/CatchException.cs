using CinemaDAL.Models;
using System.Net;

namespace CinemaAPI.Middleware
{
    public class CatchException
    {
        private readonly RequestDelegate _next;

        public CatchException(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Request.ContentType = "application/json";
                //context.Request.ContentType = "text/plain";
                await context.Response.WriteAsJsonAsync(ex.Message);
            }
        }

    }
}
