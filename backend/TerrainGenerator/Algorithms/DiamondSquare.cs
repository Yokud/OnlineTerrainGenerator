namespace TerrainGenerator
{
    public class DiamondSquare : ILandGenerator
    {
        int _seed;
        float _roughness;
        Random _random;

        public DiamondSquare(float roughness = 0.5f, int? seed = null)
        {
            Roughness = roughness;

            _seed = seed ?? Environment.TickCount;
            _random = new Random(_seed);
        }

        public int Seed
        {
            get => _seed;
            set
            {
                _seed = value;
                _random = new Random(_seed);
            }
        }

        public float Roughness
        {
            get => _roughness;
            set => _roughness = value >= 0 && value <= 1 ? value : throw new Exception("Roughness factor must be between 0 and 1");
        }

        public float[,] GenMap(int width, int height)
        {
            if (width != height)
                throw new ArgumentException("Sizes of map must be the same for this method");

            if (Math.Log2(width - 1) % 1 >= 1e-6)
                throw new ArgumentException("Sizes of map must be equal 2^n + 1 for this method");

            var size = width;
            var max = size - 1;
            var map = new float[size, size];

            map[0, 0] = RandomFloat(max);
            map[max, 0] = RandomFloat(max);
            map[max, max] = RandomFloat(max);
            map[0, max] = RandomFloat(max);

            for (var currSize = max; currSize > 1; currSize /= 2)
            {
                int x, y, half = currSize / 2;
                var scale = Roughness * currSize;

                for (y = half; y <= max; y += currSize)
                    for (x = half; x <= max; x += currSize)
                    {
                        Square(x, y, half, RandomFloat(scale));

                        Diamond(x, y - half, half, RandomFloat(scale));
                        Diamond(x - half, y, half, RandomFloat(scale));
                        Diamond(x, y + half, half, RandomFloat(scale));
                        Diamond(x + half, y, half, RandomFloat(scale));
                    }           
            }

            return map;

            void Diamond(int x, int y, int size, float offset)
            {
                var ave = new[] { (x, y - size), (x + size, y), (x, y + size), (x - size, y) }.Where(i => i.Item1 >= 0 && i.Item1 <= max && i.Item2 >= 0 && i.Item2 <= max).Select(i => map[i.Item1, i.Item2]).Average();
                map[x, y] = ave + offset;
            }

            void Square(int x, int y, int size, float offset)
            {
                var ave = new[] { (x - size, y - size), (x + size, y + size), (x - size, y + size), (x + size, y - size) }.Where(i => i.Item1 >= 0 && i.Item1 <= max && i.Item2 >= 0 && i.Item2 <= max).Select(i => map[i.Item1, i.Item2]).Average();
                map[x, y] = ave + offset;
            }
        }

        float RandomFloat(float x)
        {
            var offset = _random.NextSingle() * 2 * x - x;
            var sign = Math.Sign(offset);
            return (float)(sign * Math.Pow(Math.Abs(offset), 1 / Math.Sqrt(Roughness)));
        }
    }
}
