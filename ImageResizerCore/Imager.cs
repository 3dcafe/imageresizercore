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

        private static readonly string[] suffixes = new string[] {
            ".png",
            ".jpg",
            ".jpeg"
        };

        const string folder = "cache";
        private readonly RequestDelegate _next;
        public ImageResizerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
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

                        int height = int.Parse(_params["h"]);
                        int width = int.Parse(_params["w"]);
                        string p_ave = string.Format(
                            "{0}\\{1}_{3}_{4}{2}",
                            _workFolder, 
                            fileName,
                            ex,
                            height,
                            width);
                        if(!File.Exists(p_ave))
                        {
                            Image.FromFile(fileInfo.PhysicalPath).ResizeImageAndRatio(width, height).Save(p_ave);
                        }
                        httpContext.Response.ContentType = "image/jpeg";
                        byte[] imgdata = System.IO.File.ReadAllBytes(p_ave);
                        await httpContext.Response.Body.WriteAsync(imgdata, 0, imgdata.Length);
                        return;
                    }
                }
            }
            await _next(httpContext);
        }
    }
}
