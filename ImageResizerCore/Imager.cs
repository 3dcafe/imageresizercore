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

            var _params = httpContext.Request.Query;
            string str = "";
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
                    httpContext.Response.ContentType = "image/jpeg";
                    string p = @"C:\Users\gs3d\Source\Repos\openkey\OpenKey\wwwroot\2.jpg";
                    byte[] imgdata = System.IO.File.ReadAllBytes(p);
                    httpContext.Response.Body.Write(imgdata, 0, imgdata.Length);

                    //await httpContext.Response.Body.WriteByte(imgdata,0, imgdata.Length);
                    // await httpContext.Response.WriteAsync(Buffer,0,
                    //await httpContext.Response.WriteAsync("Invalid User Key");
                    //httpContext.Response.Redirect("/1.jpg");
                    return;
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
