using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TerrainGenerator;
using static OnlineTerrainGeneratorWebAPI.HeightMapParser.HeightMapParser;

namespace OnlineTerrainGeneratorWebAPI.HeightMapLogic
{
    public class HeightMapLogic
    {
        HeightMap _heightMap;

        private const int Size = 512;
        public HeightMapLogic() { }

        public HeightMapLogic(string JsonString)
        {
            HeigthMapParameters parameters = JsonParser(JsonString);
            var optns = parameters.options;
            ILandGenerator landGenerator;

            var expression = HeigthMapFunction(parameters.func);

            switch (parameters.alg)
            {
                case Algoritm.DiamondSquare:
                    landGenerator = new DiamondSquare(optns[0], (int)optns[1]);
                    _heightMap = new HeightMap(Size + 1, Size + 1, landGenerator, new NoiseExpresion(expression));
                    break;
                case Algoritm.PerlinNoise:
                    landGenerator = new PerlinNoise((int)optns[0], (int)optns[1], optns[2], optns[3], (int)optns[4]);
                    _heightMap = new HeightMap(Size, Size, landGenerator, new NoiseExpresion(expression));
                    break;
                case Algoritm.SimplexNoise:
                    landGenerator = new SimplexNoise((int)optns[0], (int)optns[1], optns[2], optns[3], (int)optns[4]);
                    _heightMap = new HeightMap(Size, Size, landGenerator, new NoiseExpresion(expression));
                    break;
                default:
                    break;
            }
        }
    }
}
