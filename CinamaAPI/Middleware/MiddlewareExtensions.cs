﻿namespace CinemaAPI.Middleware
{
    

    /// <summary>
    /// creo un estensione
    /// </summary>
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder MySaveChangeOnDB(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SaveChangeOnDB>();
        }
    }

}
