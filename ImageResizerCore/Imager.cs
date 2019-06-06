using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using ImageResizerCore.Utils;

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
        const string folder = "cache";
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

            var provider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));

            var fileInfo = provider.GetFileInfo(httpContext.Request.Path);
            if (fileInfo.Exists)
            {
                var ex = System.IO.Path.GetExtension(fileInfo.PhysicalPath);
                var mineType = Utils.ExMineType.GetMimeType(ex);
                if(Utils.ExMineType.IsImageHasBenResize(mineType))
                {
                    httpContext.Response.ContentType = "image/jpeg";
#warning review code
                    string _workFolder = string.Format("{0}\\wwwroot\\{1}", Directory.GetCurrentDirectory(), folder);

                    if (Directory.Exists(_workFolder)) {
                        Directory.CreateDirectory(_workFolder);
                    }
#warning validate params request
                    if(_params.Count>0) {
#warning check on exist or return
                        string fileName = System.IO.Path.GetFileName(fileInfo.PhysicalPath).Replace(ex, string.Empty);
                        string p_ave = string.Format(
                            "{0}\\{1}_{3}_{4}{2}",
                            _workFolder, 
                            fileName,
                            ex,
                            _params["h"],
                            _params["w"]);

                        int height = int.Parse(_params["h"]);
                        int width = int.Parse(_params["w"]);

                        Image.FromFile(fileInfo.PhysicalPath).Resize(width, height).Save(p_ave);

                        httpContext.Response.ContentType = "image/jpeg";
                        byte[] imgdata = System.IO.File.ReadAllBytes(p_ave);
                        httpContext.Response.Body.Write(imgdata, 0, imgdata.Length);
                        return;
                    }
                }
            }
            await _next(httpContext);
        }
    }
}
