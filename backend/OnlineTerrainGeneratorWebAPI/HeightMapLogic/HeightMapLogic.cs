using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TerrainGenerator;
using static OnlineTerrainGeneratorWebAPI.HeightMapParser.HeightMapParser;

namespace OnlineTerrainGeneratorWebAPI.Logic
{
    public class HeightMapLogic
    {
        HeightMap? _heightMap;

        private const int Size = 513;
        private static readonly ColorScheme s_colorScheme = new() { new ColorSchemeUnit(Color.DarkBlue, 75),
                                                                    new ColorSchemeUnit(Color.Blue, 135),
                                                                    new ColorSchemeUnit(Color.Yellow, 150),
                                                                    new ColorSchemeUnit(Color.LightGreen, 205),
                                                                    new ColorSchemeUnit(Color.DarkGray, 240),
                                                                    new ColorSchemeUnit(Color.White, 255)};

        private static ILandGenerator? CreateLandGenerator(GenerationAlgorithm algorithm, float?[] options) => algorithm switch
        {
            GenerationAlgorithm.DiamondSquare => new DiamondSquare(options[0] ?? DiamondSquare.DefaultRoughness, (int?)options[1]),
            GenerationAlgorithm.PerlinNoise => new PerlinNoise((int)options[0], options[1].HasValue ? (int)options[1]!.Value : PerlinNoise.DefaultOctaves, options[2] ?? PerlinNoise.DefaultLacunarity, options[3] ?? PerlinNoise.DefaultPersistense, (int?)options[4]),
            GenerationAlgorithm.SimplexNoise => new SimplexNoise((int)options[0], options[1].HasValue ? (int)options[1]!.Value : SimplexNoise.DefaultOctaves, options[2] ?? SimplexNoise.DefaultLacunarity, options[3] ?? SimplexNoise.DefaultPersistense, (int?)options[4]),
            _ => null,
        };

        public void CreateHeightMap(string jsonString)
        {
            var parameters = JsonParser(jsonString);
            var optns = parameters.AlgorithmParams;
            var landGenerator = CreateLandGenerator(parameters.Algorithm, parameters.AlgorithmParams);

            _heightMap = new HeightMap(Size, Size, landGenerator, parameters.NoiseExpression is not null ? new NoiseExpresion(parameters.NoiseExpression) : null);

        }

        public void UpdateHeightMap(string jsonString)
        {
            var parameters = JsonParser(jsonString);
            var optns = parameters.AlgorithmParams;
            _heightMap!.LandGenerator = CreateLandGenerator(parameters.Algorithm, parameters.AlgorithmParams);
            _heightMap!.NoiseExpression = parameters.NoiseExpression is not null ? new NoiseExpresion(parameters.NoiseExpression) : null;
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
