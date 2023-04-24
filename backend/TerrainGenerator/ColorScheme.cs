namespace TerrainGenerator
{
    public readonly struct ColorSchemeUnit
    {
        public readonly Color Color;
        public readonly byte GrayscaleValue;

        public ColorSchemeUnit(Color color, byte grayscaleValue)
        {
            Color = color;
            GrayscaleValue = grayscaleValue;
        }
    }

    public class ColorScheme : List<ColorSchemeUnit>
    {
        public Color GetColorFromGrayscale(byte grayscaleValue)
        {
            var list = this.Where(c => c.GrayscaleValue >= grayscaleValue);

            var colorSchemeUnit = list != null ? list.ElementAt(0) : this[^1];

            return colorSchemeUnit.Color;
        }
    }
}
