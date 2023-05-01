namespace TerrainGenerator
{
    /// <summary>
    /// Делегат функции преобразования карты высот
    /// </summary>
    /// <param name="x">Входное значение</param>
    /// <returns>Значение, соответствующий результату функции преобразования</returns>
    /// <remarks>Пример: x => x * x + 2 * x - 0.3</remarks>
    public delegate float NoiseExpresion(float x);

    /// <summary>
    /// Интерфейс алгоритма генерации ландшафта
    /// </summary>
    public interface ILandGenerator : IEquatable<ILandGenerator>
    {
        /// <summary>
        /// Зерно генерации
        /// </summary>
        int Seed { get; set; }

        /// <summary>
        /// Генерация шумовой карты размером <paramref name="width"/> на <paramref name="height"/>
        /// </summary>
        /// <param name="width">Ширина шумовой карты</param>
        /// <param name="height">Высота шумовой карты</param>
        /// <returns></returns>
        float[,] GenMap(int width, int height);
    }

    /// <summary>
    /// Карта высот
    /// </summary>
    public class HeightMap
    {
        int _width, _height;
        NoiseExpresion? _expresion;
        ILandGenerator _generator;
        float[,] _noiseMap;

        /// <summary>
        /// Инициализация карты высот
        /// </summary>
        /// <param name="width">Ширина карты высот</param>
        /// <param name="height">Высота карты высот</param>
        /// <param name="lg">Алгоритм генерации ландшафта</param>
        /// <param name="exp">Функция преобразования карты высот</param>
        public HeightMap(int width, int height, ILandGenerator lg, NoiseExpresion? exp = null)
        {
            Width = width;
            Height = height;
            _generator = lg;
            _expresion = exp;

            GenMap();
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="h">Карта высот</param>
        public HeightMap(HeightMap h)
        {
            Width = h.Width;
            Height = h.Height;
            _noiseMap = h._noiseMap;
            LandGenerator = h.LandGenerator;
            Seed = h.Seed;
        }

        /// <summary>
        /// Ширина карты высот
        /// </summary>
        public int Width
        {
            get => _width;
            set => _width = value > 0 ? value : throw new Exception("Width is positive value");
        }

        /// <summary>
        /// Высота карты высот
        /// </summary>
        public int Height
        {
            get => _height;
            set => _height = value > 0 ? value : throw new Exception("Height is positive value");
        }

        /// <summary>
        /// Зерно генерации
        /// </summary>
        public int Seed
        {
            get => LandGenerator.Seed;
            set => LandGenerator.Seed = value;
        }

        /// <summary>
        /// Алгоритм генерации ландшафта
        /// </summary>
        public ILandGenerator LandGenerator
        {
            get => _generator;
            set
            {
                _generator = value;
                GenMap();
            }
        }

        /// <summary>
        /// Функция преобразования карты высот
        /// </summary>
        public NoiseExpresion? NoiseExpression
        {
            get => _expresion;
            set
            {
                _expresion = value;
                GenMap();
            }
        }

        /// <summary>
        /// Шумовая карта
        /// </summary>
        public float[,] RawData => _noiseMap;

        /// <summary>
        /// Возвращает минимальное и максимальное значения в шумовой карте
        /// </summary>
        /// <returns>Минимальное и максимальное значения в шумовой карте</returns>
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

        /// <summary>
        /// Нормализация карты высот
        /// </summary>
        /// <remarks>Преобразование значений шумовой карты в значения от 0 до 1</remarks>
        void Normalize()
        {
            var (h_min, h_max) = MinMax();
            var delta = h_max - h_min;

            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                    _noiseMap[i, j] = (_noiseMap[i, j] - h_min) / delta;
        }

        /// <summary>
        /// Генерация ландшафта
        /// </summary>
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

        /// <summary>
        /// Генерация карты высот. На основе сгенерированной шумовой карты создается изображение в оттенках серого
        /// </summary>
        /// <returns>Изображение в оттенках серого, отражающее рельеф ландшафта</returns>
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

        /// <summary>
        /// Генерация разукрашенной карты высот. На основе сгенерированной шумовой карты создается изображение с цветами из цветовой схемы
        /// </summary>
        /// <param name="colorScheme">Цветовая схема</param>
        /// <returns>Разукрашенная карта высот</returns>
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
