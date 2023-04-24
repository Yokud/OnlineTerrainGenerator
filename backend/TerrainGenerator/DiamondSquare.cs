namespace TerrainGenerator
{
    public class DiamondSquare : ILandGenerator
    {
        int _seed;
        float _roughness;

        public DiamondSquare(float roughness = 0.5f, int? seed = null)
        {
            _roughness = roughness;

            _seed = seed ?? Environment.TickCount;
        }

        public int Seed
        {
            get => _seed;
            set => _seed = value;
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

            map[0, 0] = max;
            map[max, 0] = max / 2;
            map[max, max] = 0;
            map[0, max] = max / 2;

            Divide(max);

            return map;

            void Divide(int size)
            {
                int x, y, half = size / 2;
                var scale = Roughness * size;

                if (half < 1)
                    return;

                var rd = new Random(_seed);

                for (y = half; y < max; y += size)
                    for (x = half; x < max; x += size)
                        Square(x, y, half, rd.NextSingle() * 2 * scale - scale);

                for (y = 0; y <= max; y += half)
                    for (x = (y + half) % size; x <= max; x += size)
                        Diamond(x, y, half, rd.NextSingle() * 2 * scale - scale);

                Divide(size / 2);
            }

            void Diamond(int x, int y, int size, float offset)
            {
                var ave = new[] { (x, y - size), (x + size, y), (x, y + size), (x - size, y) }.Where(i => i.Item1 >= 0 && i.Item1 <= max && i.Item2 >= 0 && i.Item2 <= max).Select(i => map[i.Item1, i.Item2]).Average();
                map[x, y] = ave + offset;
            }

            void Square(int x, int y, int size, float offset)
            {
                var ave = new[] { (x - size, y - size), (x + size, y - size), (x + size, y + size), (x - size, y + size) }.Where(i => i.Item1 >= 0 && i.Item1 <= max && i.Item2 >= 0 && i.Item2 <= max).Select(i => map[i.Item1, i.Item2]).Average();
                map[x, y] = ave + offset;
            }
        }
    }
}
