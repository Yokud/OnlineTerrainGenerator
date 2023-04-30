namespace TerrainGenerator
{
    public delegate float NoiseExpresion(float x);

    public interface ILandGenerator : IEquatable<ILandGenerator>
    {
        int Seed { get; set; }
        float[,] GenMap(int width, int height);
    }

    public class HeightMap
    {
        int _width, _height;
        NoiseExpresion? _expresion;
        ILandGenerator _generator;
        float[,] _noiseMap;

        public HeightMap(int width, int height, ILandGenerator lg, NoiseExpresion? exp = null)
        {
            Width = width;
            Height = height;
            _generator = lg;
            _expresion = exp;

            GenMap();
        }

        public HeightMap(HeightMap h)
        {
            Width = h.Width;
            Height = h.Height;
            _noiseMap = h._noiseMap;
            LandGenerator = h.LandGenerator;
            Seed = h.Seed;
        }

        public int Width
        {
            get => _width;
            set => _width = value > 0 ? value : throw new Exception("Width is positive value");
        }

        public int Height
        {
            get => _height;
            set => _height = value > 0 ? value : throw new Exception("Height is positive value");
        }

        public int Seed
        {
            get => LandGenerator.Seed;
            set => LandGenerator.Seed = value;
        }

        public ILandGenerator LandGenerator
        {
            get => _generator;
            set
            {
                _generator = value;
                GenMap();
            }
        }

        public NoiseExpresion? NoiseExpression
        {
            get => _expresion;
            set
            {
                _expresion = value;
                GenMap();
            }
        }

        public float[,] RawData => _noiseMap;

        public float this[int i, int j]
        {
            get => _noiseMap[i, j];
            set => _noiseMap[i, j] = value;
        }

        private (float, float) MinMax()
        {
            var max = _noiseMap[0, 0];
            var min = max;

            for (var i = 0; i < _width; i++)
                for (var j = 0; j < _height; j++)
                {
                    min = _noiseMap[i, j] < min ? _noiseMap[i, j] : min;
                    max = _noiseMap[i, j] > max ? _noiseMap[i, j] : max;
                }

            return (min, max);
        }

        void Normalize()
        {
            var (h_min, h_max) = MinMax();
            var delta = h_max - h_min;

            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                    _noiseMap[i, j] = (_noiseMap[i, j] - h_min) / delta;
        }

        public void GenMap()
        {
            _noiseMap = LandGenerator.GenMap(Width, Height);

            if (NoiseExpression != null)
            {
                Normalize();

                for (var i = 0; i < _width; i++)
                    for (var j = 0; j < _height; j++)
                        _noiseMap[i, j] = NoiseExpression(_noiseMap[i, j]);
            }
        }

        public Image<Rgba32> GetGrayscaledImage()
        {
            Normalize();

            var img = new Image<Rgba32>(Width, Height);

            img.ProcessPixelRows(pixelAccessor =>
            {
                for (var i = 0; i < _width; i++)
                {
                    var pixelRow = pixelAccessor.GetRowSpan(i);
                    for (var j = 0; j < _height; j++)
                        pixelRow[j] = Color.FromRgba((byte)(_noiseMap[i, j] * 255), (byte)(_noiseMap[i, j] * 255), (byte)(_noiseMap[i, j] * 255), 255);
                }
            });

            return img;
        }

        public Image<Rgba32> GetColoredImage(ColorScheme colorScheme)
        {
            Normalize();

            var img = new Image<Rgba32>(Width, Height);

            img.ProcessPixelRows(pixelAccessor =>
            {
                for (var i = 0; i < _width; i++)
                {
                    var pixelRow = pixelAccessor.GetRowSpan(i);
                    for (var j = 0; j < _height; j++)
                        pixelRow[j] = colorScheme.GetColorFromGrayscale((byte)(_noiseMap[i, j] * 255));
                }
            });

            return img;
        }

        public override bool Equals(object? obj)
        {
            return obj is HeightMap other && Width == other.Width && Height == other.Height && LandGenerator.Equals(other.LandGenerator) && NoiseExpression?.Method.GetMethodBody() == other.NoiseExpression?.Method.GetMethodBody();
        }
    }
}
