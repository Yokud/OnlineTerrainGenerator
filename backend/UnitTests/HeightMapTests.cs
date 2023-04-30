using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace UnitTests
{
    public class HeightMapTests
    {
        [Fact]
        public void GetGrayscaledTest()
        {
            var hm = new HeightMap(33, 33, new DiamondSquare(seed: 123));

            var act = hm.GetGrayscaledImage();

            Assert.True(act is not null);
        }

        [Fact]
        public void GetColoredTest()
        {
            var hm = new HeightMap(33, 33, new DiamondSquare(seed: 123));
            var colorScheme = new ColorScheme()
            {
                new ColorSchemeUnit(Color.DarkBlue, 75),
                new ColorSchemeUnit(Color.Blue, 135),
                new ColorSchemeUnit(Color.Yellow, 150),
                new ColorSchemeUnit(Color.LightGreen, 205),
                new ColorSchemeUnit(Color.DarkGray, 240),
                new ColorSchemeUnit(Color.White, 255)
            };

            var act = hm.GetColoredImage(colorScheme);

            Assert.True(act is not null);
        }

        [Fact]
        public void DiamondSquareTest()
        {
            var hm = new HeightMap(3, 3, new DiamondSquare(seed: 123));
            var exp = new float[9] { 2.549f, 1.043f, 1.366f, 1.340f, 2.071f, 0.861f, 1.998f, 1.180f, 0.964f };

            var act = hm.RawData.Cast<float>().Select(x => (float)Math.Round(x, 3)).ToArray();

            Assert.True(Enumerable.SequenceEqual(exp, act));
        }

        [Fact]
        public void PerlinNoiseTest()
        {
            var hm = new HeightMap(3, 3, new PerlinNoise(2, seed: 123));
            var exp = new float[9] { 0f, 0.199f, 0f, 0.07f, 0.113f, 0.081f, 0f, -0.124f, 0f };

            var act = hm.RawData.Cast<float>().Select(x => (float)Math.Round(x, 3)).ToArray();

            Assert.True(Enumerable.SequenceEqual(exp, act));
        }

        [Fact]
        public void SimplexNoiseTest()
        {
            var hm = new HeightMap(3, 3, new SimplexNoise(2, seed: 123));
            var exp = new float[9] { 0f, -0.78f, 0.113f, -0.559f, -0.527f, -0.048f, 0.113f, -0.245f, -0.755f };

            var act = hm.RawData.Cast<float>().Select(x => (float)Math.Round(x, 3)).ToArray();

            Assert.True(Enumerable.SequenceEqual(exp, act));
        }
    }
}
