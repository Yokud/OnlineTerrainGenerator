using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace OnlineTerrainGeneratorWebAPI.Logic
{
    /// <summary>
    /// Класс, отвечающий за создание URL
    /// </summary>
    public class UrlCreator
    {
        /// <summary>
        /// Среда размещения
        /// </summary>
        readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// Инициализация класса, отвечающего за создание URL
        /// </summary>
        /// <param name="hostingEnvironment">Среда размещения</param>
        public UrlCreator(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Создает URL изображения
        /// </summary>
        /// <param name="request">HTTP запрос</param>
        /// <param name="image">Изображение</param>
        /// <param name="filename">Имя файла</param>
        /// <returns>URL сохраненного изображенния</returns>
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
