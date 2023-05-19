using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;

namespace CinemaAPI.Middleware
{
    public class ExceptionHandlerCustom
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerCustom(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerCustom>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(ex, context);
            }
        }

        // I realize this function looks pointless, but it will have more meat to it eventually.
        public async Task HandleException(Exception ex, HttpContext context)
        {
            if (ex is ArgumentException argEx)
            {
                _logger.LogError(0, argEx, argEx.Message);
            }
            else if (ex is InvalidOperationException ioEx)
            {
                _logger.LogError(0, ioEx, "An Invalid Operation Exception occurred. This is usually caused by a database call that expects "
                    + "one result, but receives none or more than one.");
            }
            //else if (ex is System.Data SqlException sqlEx)
            //{
            //    _logger.LogError(0, sqlEx, $"A SQL database exception occurred. Error Number {sqlEx.Number}");
            //}
            else if (ex is NullReferenceException nullEx)
            {
                _logger.LogError(0, nullEx, $"A Null Reference Exception occurred. Source: {nullEx.Source}.");
            }
            else if (ex is DbUpdateConcurrencyException dbEx)
            {
                _logger.LogError(0, dbEx, "A database error occurred while trying to update your item. This is usually due to someone else modifying the item since you loaded it.");
            }
            else
            {
                _logger.LogError(0, ex, "An unhandled exception has occurred.");
            }

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Request.ContentType = "application/json";
            //context.Request.ContentType = "text/plain";
            await context.Response.WriteAsJsonAsync(ex.Message);
        }
    }
}
