using System.Numerics;
using System.Security.Cryptography;

namespace TerrainGenerator
{
    public class SimplexNoise : ILandGenerator
    {
        const float K1 = 0.366025404f; // (sqrt(3) - 1) / 2;
        const float K2 = 0.211324865f; // (3 - sqrt(3)) / 6;

        int _scale, _octaves, _seed;
        float _lacunarity, _persistence;

        readonly int[] _seedNums;
        static readonly Vector2[] s_gradients = new Vector2[]
        {
            new Vector2(1, 1),
            new Vector2(-1, 1),
            new Vector2(1, -1),
            new Vector2(-1, -1),
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1),
            new Vector2(0, 1),
            new Vector2(0, -1),
        };

        public SimplexNoise(int scale, int octaves = 1, float lacunarity = 2f, float persistence = 0.5f, int? seed = null)
        {
            Scale = scale;
            Octaves = octaves;
            Lacunarity = lacunarity;
            Persistence = persistence;

            Seed = seed ?? Environment.TickCount;
            var rd = new Random(Seed);

            _seedNums = new int[256];
            for (var i = 0; i < _seedNums.Length; i++)
                _seedNums[i] = rd.Next(0, s_gradients.Length);
        }

        public int Scale
        {
            get => _scale;
            set => _scale = value > 0 ? value : throw new Exception("Scale is positive value");
        }

        public int Octaves
        {
            get => _octaves;
            set => _octaves = value > 0 ? value : throw new Exception("Octaves is positive value");
        }

        public float Lacunarity
        {
            get => _lacunarity;
            set => _lacunarity = value > 0 ? value : throw new Exception("Lacunarity is positive value");
        }

        public float Persistence
        {
            get => _persistence;
            set => _persistence = value > 0 ? value : throw new Exception("Persistence is positive value");
        }

        public int Seed
        {
            get => _seed;
            set => _seed = value;
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
                        map[i, j] += GenNoise(i, j) * amplitude;

                        max_amp += amplitude;
                        amplitude *= _persistence;
                        _scale = (int)Math.Round(_scale / _lacunarity, MidpointRounding.AwayFromZero);
                        temp_octs--;
                    }

                    map[i, j] /= max_amp;
                    _scale = temp_scale;
                }

            return map;
        }

        float GenNoise(int x, int y)
        {
            float n0 = 0f, n1 = 0f, n2 = 0f;

            var xScaled = x * Scale / (float)Math.Sqrt(3);
            var yScaled = y * Scale / (float)Math.Sqrt(3);
            float s = (xScaled + yScaled) * K1,
                i = (float)Math.Floor(xScaled + s),
                j = (float)Math.Floor(yScaled + s),
                t = (i + j) * K2,
                X0 = i - t, // Unskew the cell origin back to (x,y) space
                Y0 = j - t,
                x0 = xScaled - X0, // The x,y distances from the cell origin
                y0 = yScaled - Y0;

            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if (x0 > y0)
            {
                i1 = 1;
                j1 = 0;
            } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            else
            {
                i1 = 0;
                j1 = 1;
            }

            // upper triangle, YX order: (0,0)->(0,1)->(1,1)
            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6
            var x1 = x0 - i1 + K2;
            var y1 = y0 - j1 + K2;
            var x2 = x0 - 1.0f + 2.0f * K2;
            var y2 = y0 - 1.0f + 2.0f * K2;

            var t0 = 0.5f - x0 * x0 - y0 * y0;
            if (t0 >= 0)
            {
                var g0 = GetGradient(i, j);
                t0 *= t0;
                n0 = t0 * t0 * (g0.X * x0 + g0.Y * y0);
            }

            var t1 = 0.5f - x1 * x1 - y1 * y1;
            if (t1 >= 0)
            {
                var g1 = GetGradient(i + i1, j + j1);
                t1 *= t1;
                n1 = t1 * t1 * (g1.X * x1 + g1.Y * y1);
            }

            var t2 = 0.5f - x2 * x2 - y2 * y2;
            if (t2 >= 0)
            {
                var g2 = GetGradient(i + 1, j + 1);
                t2 *= t2;
                n2 = t2 * t2 * (g2.X * x2 + g2.Y * y2);
            }

            return 70.0f * (n0 + n1 + n2);
        }

        private Vector2 GetGradient(float x, float y)
        {
            var hash = BitConverter.ToInt64(SHA512.HashData(BitConverter.GetBytes(x).Concat(BitConverter.GetBytes(y)).ToArray()));
            return s_gradients[_seedNums[Math.Abs(hash) % _seedNums.Length]];
        }
    }
}
