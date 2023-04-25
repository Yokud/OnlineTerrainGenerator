namespace TerrainGenerator
{
    public readonly struct ColorSchemeUnit : IComparable<ColorSchemeUnit>
    {
        public readonly Color Color;
        public readonly byte GrayscaleValue;

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

    public class ColorScheme : List<ColorSchemeUnit>
    {
        public Color GetColorFromGrayscale(byte grayscaleValue)
        {
            var list = this.Where(c => c.GrayscaleValue >= grayscaleValue);

            var colorSchemeUnit = list != null ? list.Min() : this[^1];

            return colorSchemeUnit.Color;
        }
    }
}
