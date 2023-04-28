using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TerrainGenerator;
using static OnlineTerrainGeneratorWebAPI.HeightMapParser.HeightMapParser;

namespace OnlineTerrainGeneratorWebAPI.HeightMapLogic
{
    public class HeightMapLogic
    {
        HeightMap _heightMap;

        private const int Size = 513;
        private static readonly ColorScheme colorScheme = new() { new ColorSchemeUnit(Color.DarkBlue, 75),
                                                                              new ColorSchemeUnit(Color.Blue, 135),
                                                                              new ColorSchemeUnit(Color.Yellow, 150),
                                                                              new ColorSchemeUnit(Color.LightGreen, 205),
                                                                              new ColorSchemeUnit(Color.DarkGray, 240),
                                                                              new ColorSchemeUnit(Color.White, 255)};

        private static ILandGenerator CreateLandGenerator(GenerationAlgoritm algoritm, float[] optns)
        {
            ILandGenerator landGenerator = null;
            switch (algoritm)
            {
                case GenerationAlgoritm.DiamondSquare:
                    landGenerator = new DiamondSquare(optns[0], (int?)optns[1]);
                    break;
                case GenerationAlgoritm.PerlinNoise:
                    landGenerator = new PerlinNoise((int)optns[0], (int)optns[1], optns[2], optns[3], (int?)optns[4]);
                    break;
                case GenerationAlgoritm.SimplexNoise:
                    landGenerator = new SimplexNoise((int)optns[0], (int)optns[1], optns[2], optns[3], (int?)optns[4]);
                    break;
            }
            return landGenerator;
        }
        public HeightMapLogic(string JsonString)
        {
            var parameters = JsonParser(JsonString);
            var optns = parameters.AlgorithmParams;
            ILandGenerator landGenerator = CreateLandGenerator(parameters.Algorithm, parameters.AlgorithmParams);

            _heightMap = new HeightMap(Size, Size, landGenerator, new NoiseExpresion(parameters.NoiseExpression));

        }

        public void UpdateHeightMap(string JsonString)
        {
            var parameters = JsonParser(JsonString);
            var optns = parameters.AlgorithmParams;
            _heightMap.LandGenerator = CreateLandGenerator(parameters.Algorithm, parameters.AlgorithmParams);
            _heightMap.NoiseExpression = new NoiseExpresion(parameters.NoiseExpression);
        }

        public Image<Rgba32> GetHeightMap()
        {
            return _heightMap?.GetGrayscaledImage();
        }

        public Image<Rgba32> GetColoredHeightMap()
        {
            return _heightMap?.GetColoredImage(colorScheme);
        }


    }
}
