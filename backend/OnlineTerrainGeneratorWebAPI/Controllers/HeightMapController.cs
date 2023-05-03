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
        public ActionResult<string> GetColoredHeightMap([FromQuery] string heightMapParams)
        {
            try
            {
                _heightMapLogic.CreateHeightMap(heightMapParams);

                var img = _heightMapLogic.GetColoredHeightMap();

                if (img is null)
                    return BadRequest("Coudn't upload image.");

                var url = _urlCreator.CreateImageUrl(Request, img, "colored.png");
                return Ok(new { data = url });
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
        /// <response code="204">Карта высот ещё не была сгенерирована</response>
        [HttpGet("grayscaled")]
        public ActionResult<string> GetHeightMap()
        {
            var img = _heightMapLogic.GetHeightMap();

            return (img is null) ? NoContent() : Ok(new { data = _urlCreator.CreateImageUrl(Request, img, "grayscaled.png") });
        }

        /// <summary>
        /// Обновляет карту высот на основе указанных параметров
        /// </summary>
        /// <param name="heightMapParams">Параметры для обновления карты высот</param>
        /// <returns>URL обновленной карты высот</returns>
        /// <response code="200">URL обновленной карты высот</response>
        /// <response code="400">Не удалось обновить карту высот</response> 

        [HttpPut]
        public ActionResult<string> UpdateHeightMap([FromQuery] string heightMapParams)
        {
            try
            {
                _heightMapLogic.UpdateHeightMap(heightMapParams);

                var img = _heightMapLogic.GetColoredHeightMap();

                if (img is null)
                    return BadRequest("Coudn't upload image.");

                var url = _urlCreator.CreateImageUrl(Request, img, "colored.png");
                return Ok(new { data = url });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
