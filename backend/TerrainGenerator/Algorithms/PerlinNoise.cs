using System.Numerics;
using System.Security.Cryptography;

namespace TerrainGenerator
{
    public class PerlinNoise : ILandGenerator
    {
        public const int DefaultOctaves = 1;
        public const float DefaultLacunarity = 2f, DefaultPersistense = 0.5f;

        int _scale, _octaves, _seed;
        float _lacunarity, _persistence;

        readonly byte[] _seedNums;
        readonly Vector2[] _gradients;

        public PerlinNoise(int scale, int octaves = DefaultOctaves, float lacunarity = DefaultLacunarity, float persistence = DefaultPersistense, int? seed = null)
        {
            Scale = scale;
            Octaves = octaves;
            Lacunarity = lacunarity;
            Persistence = persistence;

            _gradients = new Vector2[256];
            for (var i = 0; i < _gradients.Length; i++)
            {
                var val = 2.0 * Math.PI / 256 * i;
                _gradients[i].X = (float)Math.Cos(val);
                _gradients[i].Y = (float)Math.Sin(val);
            }

            _seed = seed ?? Environment.TickCount;
            var rd = new Random(_seed);

            _seedNums = new byte[256];
            rd.NextBytes(_seedNums);
        }

        public int Scale
        {
            get => _scale;
            set => _scale = value > 0 ? value : throw new ArgumentException("Scale is positive value", nameof(Scale));
        }

        public int Octaves
        {
            get => _octaves;
            set => _octaves = value > 0 ? value : throw new ArgumentException("Octaves is positive value", nameof(Octaves));
        }

        public float Lacunarity
        {
            get => _lacunarity;
            set => _lacunarity = value > 0 ? value : throw new ArgumentException("Lacunarity is positive value", nameof(Lacunarity));
        }

        public float Persistence
        {
            get => _persistence;
            set => _persistence = value > 0 ? value : throw new ArgumentException("Persistence is positive value", nameof(Persistence));
        }

        public int Seed
        {
            get => _seed;
            set
            {
                _seed = value;
                var rd = new Random(_seed);
                rd.NextBytes(_seedNums);
            }
        }

        float GenNoise(int x, int y, int scale)
        {
            var pos = new Vector2((float)x / scale, (float)y / scale);

            var x0 = (float)Math.Floor(pos.X);
            var x1 = x0 + 1;
            var y0 = (float)Math.Floor(pos.Y);
            var y1 = y0 + 1;

            var g0 = GetGradient(x0, y0);
            var g1 = GetGradient(x1, y0);
            var g2 = GetGradient(x0, y1);
            var g3 = GetGradient(x1, y1);

            var d0 = new Vector2(pos.X - x0, pos.Y - y0);
            var d1 = new Vector2(pos.X - x1, pos.Y - y0);
            var d2 = new Vector2(pos.X - x0, pos.Y - y1);
            var d3 = new Vector2(pos.X - x1, pos.Y - y1);

            var sd0 = Vector2.Dot(g0, d0);
            var sd1 = Vector2.Dot(g1, d1);
            var sd2 = Vector2.Dot(g2, d2);
            var sd3 = Vector2.Dot(g3, d3);

            var sx = SmootherStep(d0.X);
            var sy = SmootherStep(d0.Y);

            var blendx1 = sd0 + sx * (sd1 - sd0);
            var blendx2 = sd2 + sx * (sd3 - sd2);
            var blendy = blendx1 + sy * (blendx2 - blendx1);

            return blendy;
        }

        public float[,] GenMap(int width, int height)
        {
            var map = new float[width, height];

            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    float amplitude = 1;
                    float max_amp = 0;
                    var temp_octs = Octaves;
                    var temp_scale = _scale;

                    while (temp_octs > 0)
                    {
                        map[i, j] += GenNoise(i, j, temp_scale) * amplitude;

                        max_amp += amplitude;
                        amplitude *= _persistence;
                        temp_scale = (int)Math.Round(temp_scale / _lacunarity, MidpointRounding.AwayFromZero);
                        temp_octs--;
                    }

                    map[i, j] /= max_amp;
                }

            return map;
        }

        private Vector2 GetGradient(float x, float y)
        {
            var hash = BitConverter.ToInt64(SHA512.HashData(BitConverter.GetBytes(x).Concat(BitConverter.GetBytes(y)).ToArray()));
            return _gradients[_seedNums[Math.Abs(hash) % _seedNums.Length]];
        }

        private static float SmootherStep(float t) => t * t * t * (6 * t * t - 15 * t + 10);
    }
}
