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

        [HttpGet("grayscaled")]
        public IActionResult GetHeightMap()
        {
            var img = _heightMapLogic.GetHeightMap();

            return (img is null) ? NoContent() : Ok(_urlCreator.CreateImageUrl(Request, img, "grayscaled.png"));
        }

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
