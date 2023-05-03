namespace UnitTests
{
    public class LogicTests
    {
        [Fact]
        public void CreateHeightMapTest()
        {
            var logic = new HeightMapLogic();
            var arg = @"{""func"":"""",""alg"":""DiamondSquare"",""options"":[""0.3"",""123""]}";
            var exp = new HeightMap(513, 513, new DiamondSquare(0.3f, 123));

            logic.CreateHeightMap(arg);

            Assert.Equal(exp, logic.HeightMap);
        }

        [Fact]
        public void UnknownAlgorithmTest()
        {
            var logic = new HeightMapLogic();
            var arg = @"{""func"":"""",""alg"":""Voronoi"",""options"":[""0.3"",""123""]}";

            Assert.Throws<ArgumentException>(() =>  logic.CreateHeightMap(arg));
        }

        [Fact]
        public void UpdateHeightMapTest()
        {
            var logic = new HeightMapLogic();
            var argOld = @"{""func"":"""",""alg"":""DiamondSquare"",""options"":[""0.3"",""123""]}";
            var argNew = @"{""func"":""x*x"",""alg"":""PerlinNoise"",""options"":[""64"","""","""","""",""123""]}";
            var exp = new HeightMap(513, 513, new PerlinNoise(64, seed: 123), new NoiseExpresion(new Func<float, float>(x => x * x)));

            logic.CreateHeightMap(argOld);
            logic.UpdateHeightMap(argNew);

            Assert.Equal(exp, logic.HeightMap);
        }
    }
}
