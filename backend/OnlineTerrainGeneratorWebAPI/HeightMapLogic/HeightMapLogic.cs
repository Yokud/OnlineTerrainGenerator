using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TerrainGenerator;
using static OnlineTerrainGeneratorWebAPI.HeightMapParser.HeightMapParser;

namespace OnlineTerrainGeneratorWebAPI.Logic
{
    /// <summary>
    /// Класс, отвечающий за создание карты высот
    /// </summary>
    public class HeightMapLogic
    {
        /// <summary>
        /// Размерность карты высот
        /// </summary>
        private const int Size = 513;

        /// <summary>
        /// Цветовая схема для создания карты высот
        /// </summary>
        private static readonly ColorScheme s_colorScheme = new() { new ColorSchemeUnit(Color.DarkBlue, 75),
                                                                    new ColorSchemeUnit(Color.Blue, 135),
                                                                    new ColorSchemeUnit(Color.Yellow, 150),
                                                                    new ColorSchemeUnit(Color.LightGreen, 205),
                                                                    new ColorSchemeUnit(Color.DarkGray, 240),
                                                                    new ColorSchemeUnit(Color.White, 255)};

        /// <summary>
        /// Сгенерированная карта высот
        /// </summary>
        public HeightMap? HeightMap { get; private set; }

        /// <summary>
        /// Создает генератор карты высот
        /// </summary>
        /// <param name=" algorithm">Алгоритм для создания генератора карты высот</param>
        /// <param name=" options">Параметры для создания генератора карты высот</param>
        private static ILandGenerator CreateLandGenerator(GenerationAlgorithm algorithm, float?[] options) => algorithm switch
        {
            GenerationAlgorithm.DiamondSquare => new DiamondSquare(options[0] ?? DiamondSquare.DefaultRoughness, (int?)options[1]),
            GenerationAlgorithm.PerlinNoise => new PerlinNoise((int)options[0], options[1].HasValue ? (int)options[1]!.Value : PerlinNoise.DefaultOctaves, options[2] ?? PerlinNoise.DefaultLacunarity, options[3] ?? PerlinNoise.DefaultPersistense, (int?)options[4]),
            GenerationAlgorithm.SimplexNoise => new SimplexNoise((int)options[0], options[1].HasValue ? (int)options[1]!.Value : SimplexNoise.DefaultOctaves, options[2] ?? SimplexNoise.DefaultLacunarity, options[3] ?? SimplexNoise.DefaultPersistense, (int?)options[4]),
            _ => throw new ArgumentException("Unknown terrain generation algorithm"),
        };

        /// <summary>
        /// Создает карту высот на основе переданного параметра
        /// </summary>
        /// <param name="jsonString">JSON строка, состоящая из алгоритма и параметров для карты высот</param>
        public void CreateHeightMap(string jsonString)
        {
            var parameters = JsonParser(jsonString);
            var optns = parameters.AlgorithmParams;
            var landGenerator = CreateLandGenerator(parameters.Algorithm, parameters.AlgorithmParams);

            HeightMap = new HeightMap(Size, Size, landGenerator, parameters.NoiseExpression is not null ? new NoiseExpresion(parameters.NoiseExpression) : null);
        }

        /// <summary>
        /// Обновляет карту высот на основе переданного параметра
        /// </summary>
        /// <param name="jsonString">JSON строка, состоящая из алгоритма и параметров для карты высот</param>
        public void UpdateHeightMap(string jsonString)
        {
            var parameters = JsonParser(jsonString);
            var optns = parameters.AlgorithmParams;
            HeightMap!.LandGenerator = CreateLandGenerator(parameters.Algorithm, parameters.AlgorithmParams);
            HeightMap!.NoiseExpression = parameters.NoiseExpression is not null ? new NoiseExpresion(parameters.NoiseExpression) : null;
        }

        /// <summary>
        // Возвращает карту высот в оттенках серого
        /// </summary>
        /// <returns>Карта высот в оттенках серого</returns>
        public Image<Rgba32>? GetHeightMap()
        {
            return HeightMap?.GetGrayscaledImage();
        }

        /// <summary>
        // Возвращает разукрашенную карту высот
        /// </summary>
        /// <returns>Разукрашенная карта высот</returns>
        public Image<Rgba32>? GetColoredHeightMap()
        {
            return HeightMap?.GetColoredImage(s_colorScheme);
        }
    }
}
