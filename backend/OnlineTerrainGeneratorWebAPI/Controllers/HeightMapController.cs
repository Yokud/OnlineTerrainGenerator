using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using OnlineTerrainGeneratorWebAPI.Logic;

namespace OnlineTerrainGeneratorWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeightMapController : Controller
    {
        HeightMapLogic _heightMapLogic;
        UrlCreator _urlCreator;

        public HeightMapController(HeightMapLogic heightMapLogic, IWebHostEnvironment webHostEnvironment)
        {
            _heightMapLogic = heightMapLogic;
            _urlCreator= new UrlCreator(webHostEnvironment);
        }

        [HttpGet]
        public IActionResult GetColoredHeightMap([FromBody] string jsonString)
        {
            _heightMapLogic.CreateHeightMap(jsonString);

            var img = _heightMapLogic.GetColoredHeightMap();

            return (img is null) ? BadRequest() : Ok(_urlCreator.CreateImageUrl(Request, img, "colored.png"));
        }

        [HttpGet]
        public IActionResult GetHeightMap()
        {

        }

        [HttpPut]
        public IActionResult UpdateHeightMap([FromBody] string jsonString)
        {

        }
    }
}
