using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using OnlineTerrainGeneratorWebAPI.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests
{
    public class IntegrLogicTests
    {
        readonly HeightMapLogic _heightMapLogic;
        readonly UrlCreator _urlCreator;
        readonly TestServer _server;

        public IntegrLogicTests()
        {
            _heightMapLogic = new HeightMapLogic();

            _server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _urlCreator = new UrlCreator(_server.Services.GetRequiredService<IWebHostEnvironment>());
        }

        [Fact]
        public void GetHeightMapUrlTest()
        {
            var arg = @"{""func"":"""",""alg"":""DiamondSquare"",""options"":[""0.3"",""123""]}";
            var exp = "https://localhost:8000/images/grayscaled.png";

            _heightMapLogic.CreateHeightMap(arg);
            var img = _heightMapLogic.GetHeightMap();
            var act = _urlCreator.CreateImageUrl(new TestRequest("localhost:8000", "https", ""), img, "grayscaled.png");

            Assert.Equal(exp, act);
        }

        [Fact]
        public void GetColoredHeightMapUrlTest()
        {
            var arg = @"{""func"":"""",""alg"":""DiamondSquare"",""options"":[""0.3"",""123""]}";
            var exp = "https://localhost:8000/images/colored.png";

            _heightMapLogic.CreateHeightMap(arg);
            var img = _heightMapLogic.GetColoredHeightMap();
            var act = _urlCreator.CreateImageUrl(new TestRequest("localhost:8000", "https", ""), img, "colored.png");

            Assert.Equal(exp, act);
        }
    }
}