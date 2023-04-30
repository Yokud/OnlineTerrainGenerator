namespace TerrainGenerator
{
    public class SimplexNoise : ILandGenerator
    {
        public const int DefaultOctaves = 1;
        public const float DefaultLacunarity = 2f, DefaultPersistense = 0.5f;

        int _scale, _octaves, _seed;
        float _lacunarity, _persistence;

        readonly byte[] _seedNums;

        public SimplexNoise(int scale, int octaves = DefaultOctaves, float lacunarity = DefaultLacunarity, float persistence = DefaultPersistense, int? seed = null)
        {
            Scale = scale;
            Octaves = octaves;
            Lacunarity = lacunarity;
            Persistence = persistence;

            _seed = seed ?? Environment.TickCount;
            var rd = new Random(_seed);

            _seedNums = new byte[512];
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

        public float[,] GenMap(int width, int height)
        {
            var map = new float[width, height];

            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    float amplitude = 1;
                    float max_amp = 0;
                    var temp_octs = Octaves;
                    var temp_scale = (float)_scale;

                    while (temp_octs > 0)
                    {
                        map[i, j] += Generate(i / temp_scale, j / temp_scale) * amplitude;

                        max_amp += amplitude;
                        amplitude *= _persistence;
                        temp_scale = (int)Math.Round(temp_scale / _lacunarity, MidpointRounding.AwayFromZero);
                        temp_octs--;
                    }

                    map[i, j] /= max_amp;
                }

            return map;
        }

        private float Generate(float x, float y)
        {
            const float F2 = 0.366025403f; // F2 = 0.5*(sqrt(3.0)-1.0)
            const float G2 = 0.211324865f; // G2 = (3.0-Math.sqrt(3.0))/6.0

            float n0, n1, n2; // Noise contributions from the three corners

            // Skew the input space to determine which simplex cell we're in
            var s = (x + y) * F2; // Hairy factor for 2D
            var xs = x + s;
            var ys = y + s;
            var i = FastFloor(xs);
            var j = FastFloor(ys);

            var t = (i + j) * G2;
            var X0 = i - t; // Unskew the cell origin back to (x,y) space
            var Y0 = j - t;
            var x0 = x - X0; // The x,y distances from the cell origin
            var y0 = y - Y0;

            // For the 2D case, the simplex shape is an equilateral triangle.
            // Determine which simplex we are in.
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if (x0 > y0)
            { i1 = 1; j1 = 0; } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            else
            { i1 = 0; j1 = 1; }      // upper triangle, YX order: (0,0)->(0,1)->(1,1)

            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6

            var x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
            var y1 = y0 - j1 + G2;
            var x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
            var y2 = y0 - 1.0f + 2.0f * G2;

            // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds
            var ii = Mod(i, 256);
            var jj = Mod(j, 256);

            // Calculate the contribution from the three corners
            var t0 = 0.5f - x0 * x0 - y0 * y0;
            if (t0 < 0.0f)
                n0 = 0.0f;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * Grad(_seedNums[ii + _seedNums[jj]], x0, y0);
            }

            var t1 = 0.5f - x1 * x1 - y1 * y1;
            if (t1 < 0.0f)
                n1 = 0.0f;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * Grad(_seedNums[ii + i1 + _seedNums[jj + j1]], x1, y1);
            }

            var t2 = 0.5f - x2 * x2 - y2 * y2;
            if (t2 < 0.0f)
                n2 = 0.0f;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * Grad(_seedNums[ii + 1 + _seedNums[jj + 1]], x2, y2);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 40.0f * (n0 + n1 + n2); // TODO: The scale factor is preliminary!
        }

        private static int FastFloor(float x)
        {
            return (x > 0) ? ((int)x) : (((int)x) - 1);
        }

        private static int Mod(int x, int m)
        {
            var a = x % m;
            return a < 0 ? a + m : a;
        }

        private static float Grad(int hash, float x, float y)
        {
            var h = hash & 7;      // Convert low 3 bits of hash code
            var u = h < 4 ? x : y;  // into 8 simple gradient directions,
            var v = h < 4 ? y : x;  // and compute the dot product with (x,y).
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -2.0f * v : 2.0f * v);
        }

        public bool Equals(ILandGenerator? other) => other is SimplexNoise otherSN && Scale == otherSN.Scale && Octaves == otherSN.Octaves && Lacunarity == otherSN.Lacunarity && Persistence == otherSN.Persistence && Seed == otherSN.Seed;
    }
}
