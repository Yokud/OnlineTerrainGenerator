namespace TerrainGenerator
{
    /// <summary>
    /// Элемент цветовой схемы
    /// </summary>
    public readonly struct ColorSchemeUnit : IComparable<ColorSchemeUnit>
    {
        /// <summary>
        /// Цвет
        /// </summary>
        public readonly Color Color;

        /// <summary>
        /// Значение оттенка серого ([0; 255])
        /// </summary>
        public readonly byte GrayscaleValue;

        /// <summary>
        /// Инициализация элемента цветовой схемы
        /// </summary>
        /// <param name="color">Цвет</param>
        /// <param name="grayscaleValue">Значение оттенка серого ([0; 255])</param>
        public ColorSchemeUnit(Color color, byte grayscaleValue)
        {
            Color = color;
            GrayscaleValue = grayscaleValue;
        }

        public int CompareTo(ColorSchemeUnit other)
        {
            return GrayscaleValue - other.GrayscaleValue;
        }
    }

    /// <summary>
    /// Цветовая схема
    /// </summary>
    public class ColorScheme : List<ColorSchemeUnit>
    {
        /// <summary>
        /// На основе значения <paramref name="grayscaleValue"/> возвращает цвет, который ему соответствует
        /// </summary>
        /// <param name="grayscaleValue">Значение оттенка серого ([0; 255])</param>
        /// <returns>Цвет, соответствующий оттенку серого</returns>
        public Color GetColorFromGrayscale(byte grayscaleValue)
        {
            var list = this.Where(c => c.GrayscaleValue >= grayscaleValue);

            var colorSchemeUnit = list != null ? list.Min() : this[^1];

            return colorSchemeUnit.Color;
        }
    }
}
