using CinemaDAL.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics;

namespace CinemaAPI.Middleware
{
    public class SaveChangeOnDB
    {
        private readonly RequestDelegate _next;
        //private readonly CinemaContext _ctx;

        public SaveChangeOnDB(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, CinemaContext ctx)
        {
            using var trans = ctx.Database.BeginTransaction();
            try
            {
                //Debug.WriteLine("entra nel midleWare");
                //await context.Response.WriteAsync($"entra nel midleWare {Environment.NewLine}");

                // Call the next delegate/middleware in the pipeline.
                await _next(context);
                if(ctx.ChangeTracker.HasChanges()) 
                {
                    ctx.SaveChanges();
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();

                Debug.WriteLine(ex.Message);
                throw new Exception($"Error during _ctx.SaveChanges() nel middleware - Message: {ex.Message}", ex);
            }
            finally
            {
                // qui potrei mettere le Dispose()
            }
        }
    }
}
