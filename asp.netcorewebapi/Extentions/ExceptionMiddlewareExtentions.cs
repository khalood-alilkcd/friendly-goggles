using Contracts;
using Entities.ErrorModels;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;   

namespace asp.netcorewebapi.Extentions
{
    public static class ExceptionMiddlewareExtentions 
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError => 
            {
                appError.Run(async context => {
                    
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {

                        context.Response.StatusCode = contextFeature.Error switch
                        { 
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError,
                        };

                        logger.LogError($"Something Went Wrong : {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails
                        { 
                            StutasCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        }.ToString());
                    }
                });
            });
        }
    }
}
