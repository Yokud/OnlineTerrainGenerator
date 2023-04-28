using Microsoft.AspNetCore.Mvc;
using OnlineTerrainGeneratorWebAPI.Logic;

namespace OnlineTerrainGeneratorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeightMapController : Controller
    {
        HeightMapLogic _heightMapLogic;
        UrlCreator _urlCreator;

        public HeightMapController(HeightMapLogic heightMapLogic, IWebHostEnvironment webHostEnvironment)
        {
            _heightMapLogic = heightMapLogic;
            _urlCreator= new UrlCreator(webHostEnvironment);
        }

        [HttpGet("colored")]
        public IActionResult GetColoredHeightMap([FromQuery] string jsonString)
        {
            _heightMapLogic.CreateHeightMap(jsonString);

            var img = _heightMapLogic.GetColoredHeightMap();

            return (img is null) ? BadRequest() : Ok(_urlCreator.CreateImageUrl(Request, img, "colored.png"));
        }

        [HttpGet("grayscaled")]
        public IActionResult GetHeightMap()
        {
            var img = _heightMapLogic.GetHeightMap();

            return (img is null) ? NoContent() : Ok(_urlCreator.CreateImageUrl(Request, img, "grayscaled.png"));
        }

        [HttpPut]
        public IActionResult UpdateHeightMap([FromQuery] string jsonString)
        {
            _heightMapLogic.UpdateHeightMap(jsonString);

            var img = _heightMapLogic.GetColoredHeightMap();

            return (img is null) ? BadRequest() : Ok(_urlCreator.CreateImageUrl(Request, img, "colored.png"));
        }
    }
}
