using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TerrainGenerator;
using static OnlineTerrainGeneratorWebAPI.HeightMapParser.HeightMapParser;

namespace OnlineTerrainGeneratorWebAPI.HeightMapLogic
{
    public class HeightMapLogic
    {
        readonly HeightMap _heightMap;

        private const int Size = 513;
        private static readonly ColorScheme s_colorScheme = new() { new ColorSchemeUnit(Color.DarkBlue, 75),
                                                                    new ColorSchemeUnit(Color.Blue, 135),
                                                                    new ColorSchemeUnit(Color.Yellow, 150),
                                                                    new ColorSchemeUnit(Color.LightGreen, 205),
                                                                    new ColorSchemeUnit(Color.DarkGray, 240),
                                                                    new ColorSchemeUnit(Color.White, 255)};

        private static ILandGenerator? CreateLandGenerator(GenerationAlgorithm algorithm, float[] options)
        {
            return algorithm switch
            {
                GenerationAlgorithm.DiamondSquare => new DiamondSquare(options[0], (int?)options[1]),
                GenerationAlgorithm.PerlinNoise => new PerlinNoise((int)options[0], (int)options[1], options[2], options[3], (int?)options[4]),
                GenerationAlgorithm.SimplexNoise => new SimplexNoise((int)options[0], (int)options[1], options[2], options[3], (int?)options[4]),
                _ => null,
            };
        }
        public HeightMapLogic(string jsonString)
        {
            var parameters = JsonParser(jsonString);
            var optns = parameters.AlgorithmParams;
            var landGenerator = CreateLandGenerator(parameters.Algorithm, parameters.AlgorithmParams);

            _heightMap = new HeightMap(Size, Size, landGenerator, new NoiseExpresion(parameters.NoiseExpression));

        }

        public void UpdateHeightMap(string jsonString)
        {
            var parameters = JsonParser(jsonString);
            var optns = parameters.AlgorithmParams;
            _heightMap.LandGenerator = CreateLandGenerator(parameters.Algorithm, parameters.AlgorithmParams);
            _heightMap.NoiseExpression = new NoiseExpresion(parameters.NoiseExpression);
        }

        public Image<Rgba32>? GetHeightMap()
        {
            return _heightMap?.GetGrayscaledImage();
        }

        public Image<Rgba32>? GetColoredHeightMap()
        {
            return _heightMap?.GetColoredImage(s_colorScheme);
        }


    }
}
