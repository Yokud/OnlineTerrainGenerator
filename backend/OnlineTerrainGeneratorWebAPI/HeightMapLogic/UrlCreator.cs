using Microsoft.Extensions.Hosting.Internal;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace OnlineTerrainGeneratorWebAPI.Logic
{
    public class UrlCreator
    {
        IWebHostEnvironment _hostingEnvironment;
        public UrlCreator(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public string CreateImageUrl(HttpRequest Request, Image<Rgba32> image, string filename)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(uploadFolder, filename);
            image.SaveAsPng(new FileStream(filePath, FileMode.Create));
            string baseUrl = Request.Scheme + "://" + Request.Authority +
    Request.ApplicationPath.TrimEnd('/') + $"/images/{filename}";
        }
    }
}
