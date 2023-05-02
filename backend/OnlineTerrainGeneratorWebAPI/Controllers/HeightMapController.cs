using Microsoft.AspNetCore.Mvc;
using OnlineTerrainGeneratorWebAPI.Logic;


namespace OnlineTerrainGeneratorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeightMapController : Controller
    {
        readonly HeightMapLogic _heightMapLogic;
        readonly UrlCreator _urlCreator;

        public HeightMapController(HeightMapLogic heightMapLogic, IWebHostEnvironment webHostEnvironment)
        {
            _heightMapLogic = heightMapLogic;
            _urlCreator= new UrlCreator(webHostEnvironment);
        }

        /// <summary>
        /// Получает разукрашенную карту высот на основе указанных параметров
        /// </summary>
        /// <param name="heightMapParams">Параметры карты высот</param>
        /// <returns>URL сгенерированной карты высот</returns>
        /// <response code="200">URL сгенерированной карты высот</response>
        /// <response code="400">Не удалось получить карту высот</response> 
        [HttpGet("colored")]
        public IActionResult GetColoredHeightMap([FromQuery] string heightMapParams)
        {
            try
            {
                _heightMapLogic.CreateHeightMap(heightMapParams);

                var img = _heightMapLogic.GetColoredHeightMap();

                return (img is null) ? BadRequest("Coudn't upload image.") : Ok(_urlCreator.CreateImageUrl(Request, img, "colored.png"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получает карту высот в оттенках серого
        /// </summary>
        /// <returns>URL сгенерированной карты высот</returns>
        /// <response code="200">URL сгенерированной карты высот</response>
        /// <response code="204">Параметры карты высот не заданы</response>
        [HttpGet("grayscaled")]
        public IActionResult GetHeightMap()
        {
            var img = _heightMapLogic.GetHeightMap();

            return (img is null) ? NoContent() : Ok(_urlCreator.CreateImageUrl(Request, img, "grayscaled.png"));
        }

        /// <summary>
        /// Обновляет карту высот на основе указанных параметров
        /// </summary>
        /// <param name="heightMapParams">Параметры для обновления карты высот</param>
        /// <returns>URL обновленной карты высот</returns>
        /// <response code="200">URL обновленной карты высот</response>
        /// <response code="400">Не удалось обновить карту высот</response> 

        [HttpPut]
        public IActionResult UpdateHeightMap([FromQuery] string heightMapParams)
        {
            try
            {
                _heightMapLogic.UpdateHeightMap(heightMapParams);

                var img = _heightMapLogic.GetColoredHeightMap();

                return (img is null) ? BadRequest("Coudn't upload image.") : Ok(_urlCreator.CreateImageUrl(Request, img, "colored.png"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
