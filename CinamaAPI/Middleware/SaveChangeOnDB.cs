using CinemaBL.Repository;
using CinemaDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics;
using System.Net;

namespace CinemaAPI.Middleware
{
    public class SaveChangeOnDB
    {
        private readonly RequestDelegate _next;

        public SaveChangeOnDB(RequestDelegate next)
        {
            _next = next;
        }

        //public async Task InvokeAsync(HttpContext context, CinemaContext ctx)
        //{
        //    using var trans = ctx.Database.BeginTransaction();
        //    try
        //    {
        //        //Debug.WriteLine("entra nel midleWare");
        //        //await context.Response.WriteAsync($"entra nel midleWare {Environment.NewLine}");

        //        // Call the next delegate/middleware in the pipeline.
        //        await _next(context);
        //        if (ctx.ChangeTracker.HasChanges())
        //        {
        //            try
        //            {
        //                ctx.SaveChanges();
        //                trans.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                trans.Rollback();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Debug.WriteLine(ex.Message);
        //        // cre
        //        // context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        //await context.Response.WriteAsync(ex.ToString());

        //        throw new Exception($"Error during Context.SaveChanges() nel middleware - Message: {ex.Message}", ex);
        //    }
        //    finally
        //    {
        //        // qui potrei mettere le Dispose()
        //    }
        //}

        //public async Task InvokeAsync(HttpContext context, IUnitOfWork uow)
        //{
        //    using var bt = uow.BeginTransaction();
        //    try
        //    {
        //        await _next(context);

        //        if (uow.HasChanges())
        //        {
        //            try
        //            {
        //                await uow.SaveChangesAsync();
        //                bt.Commit();    
        //            }
        //            catch (Exception ex)
        //            {
        //                bt.Rollback();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error during Context.SaveChanges() nel middleware - Message: {ex.Message}", ex);
        //    }
        //    finally
        //    {
        //        // qui potrei mettere le Dispose()
        //    }
        //}

        public async Task InvokeAsync(HttpContext context, IUnitOfWorkGeneric uow)
        {
            await _next(context);

            uow.Save();

            //try
            //{
            //    await _next(context);
            //    //await uow.SaveChangesAsync();

            //    uowg.Save();
            //}
            //catch (Exception ex)
            //{
            //    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    context.Request.ContentType = "application/json";
            //    //context.Request.ContentType = "text/plain";
            //    await context.Response.WriteAsJsonAsync(ex.Message);

            //    throw new Exception($"Error during Context.SaveChanges() nel middleware - Message: {ex.Message}", ex);
            //}
            //finally
            //{
            //    // qui potrei mettere le Dispose()
            //}
        }

    }
}
