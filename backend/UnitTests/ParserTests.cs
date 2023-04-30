namespace UnitTests
{
    public class ParserTests
    {
        [Fact]
        public void EmptyStringParseTest()
        {
            var arg = "";

            Assert.Throws<ArgumentException>(() => HeightMapParser.JsonParser(arg));
        }

        [Fact]
        public void InvalidJsonKeyParseTest()
        {
            var arg = @"{""func"":"""",""alg1"":""DiamondSquare"",""options"":[""0.3"",""123""]}";

            var act = HeightMapParser.JsonParser(arg);

            Assert.Equal(HeightMapParser.GenerationAlgorithm.Unknown, act.Algorithm);
        }

        [Fact]
        public void InvalidJsonValueParseTest()
        {
            var arg = @"{""func"":"""",""alg"":""Voronoi"",""options"":[""0.3"",""123""]}";

            var act = HeightMapParser.JsonParser(arg);

            Assert.Equal(HeightMapParser.GenerationAlgorithm.Unknown, act.Algorithm);
        }

        [Fact]
        public void ParseTest()
        {
            var arg = @"{""func1"":"""",""alg"":""DiamondSquare"",""options"":[""0.3"",""123""]}";
            var exp = new HeightMapParser.HeigthMapParams() { Algorithm = HeightMapParser.GenerationAlgorithm.DiamondSquare, AlgorithmParams = new float?[] { 0.3f, 123 } };

            var act = HeightMapParser.JsonParser(arg);

            Assert.True(exp.NoiseExpression == act.NoiseExpression && exp.Algorithm == act.Algorithm && Enumerable.SequenceEqual(exp.AlgorithmParams, act.AlgorithmParams));
        }

        [Fact]
        public void ParseFunctionTest()
        {
            var arg = "x * x";

            var act = HeightMapParser.HeigthMapFunction(arg);

            Assert.Equal(4f, act(2));
        }

        [Fact]
        public void EmptyStringParseFunctionTest()
        {
            var arg = "";

            var act = HeightMapParser.HeigthMapFunction(arg);

            Assert.Null(act);
        }
    }
}
