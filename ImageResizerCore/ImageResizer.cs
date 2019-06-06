using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageResizerCore
{
    
    public static class ImageResizer
    {
        public static IApplicationBuilder UseImageResizerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageResizerMiddleware>();
        }
    }

    public class ImageResizerMiddleware
    {
        private readonly RequestDelegate _next;
        // private readonly ILogger _logger;

        public ImageResizerMiddleware(RequestDelegate next)
        {
            _next = next;
            string st = "";
            // _logger = logFactory.CreateLogger("MyMiddleware");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // _logger.LogInformation("MyMiddleware executing..");


            /*
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            });*/

            var provider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));

            var fileInfo = provider.GetFileInfo(httpContext.Request.Path);
            if (fileInfo.Exists)
            {
                var ex = System.IO.Path.GetExtension(fileInfo.PhysicalPath);
                var mineType = Utils.ExMineType.GetMimeType(ex);
                if(Utils.ExMineType.IsImageHasBenResize(mineType))
                {
                    string str = "";
                }
            }

            //httpContext.Response.StatusCode = 500;
            //httpContext.Response.ContentType = "application/json";
            //string jsonString = JsonConvert.SerializeObject(new {state =false });
            //await httpContext.Response.WriteAsync(jsonString, Encoding.UTF8);
            //// to stop futher pipeline execution 
            //return;

            await _next(httpContext); // calling next middleware

        }
    }
}
