using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace OnlineTerrainGeneratorWebAPI.Logic
{
    public class UrlCreator
    {
        readonly IWebHostEnvironment _hostingEnvironment;

        public UrlCreator(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string CreateImageUrl(HttpRequest request, Image<Rgba32> image, string filename)
        {
            var uploadFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/images");
            var filePath = Path.Combine(uploadFolder, filename);
            Directory.CreateDirectory(uploadFolder);
            image.SaveAsPng(new FileStream(filePath, FileMode.Create));

            return request.Scheme + "://" + request.Host + request.PathBase + $"/images/{filename}";
        }
    }
}
