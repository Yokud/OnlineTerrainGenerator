using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace OnlineTerrainGeneratorWebAPI.HeightMapParser
{
    public static class HeightMapParser
    {
        public enum GenerationAlgorithm
        {
            DiamondSquare,
            PerlinNoise,
            SimplexNoise,
            Unknown
        }

        public struct HeigthMapParams
        {
            public Func<float, float> NoiseExpression;
            public GenerationAlgorithm Algorithm;
            public float?[] AlgorithmParams;
        };
        
        public static HeigthMapParams JsonParser(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                throw new ArgumentException("Source string is null or empty", nameof(jsonString));

            HeigthMapParams parameters;
            var jsonObject = JObject.Parse(jsonString);

            var func = (string)jsonObject["func"];
            var alg = (string)jsonObject["alg"];
            var options = (JArray)jsonObject["options"];

            var optionsArray = options.ToObject<float?[]>();

            parameters.NoiseExpression = HeigthMapFunction(func);
            parameters.Algorithm = alg is not null ? (GenerationAlgorithm)Enum.Parse(typeof(GenerationAlgorithm), alg) : GenerationAlgorithm.Unknown;
            parameters.AlgorithmParams = optionsArray;

            return parameters;
        }

        public static Func<float, float> HeigthMapFunction(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return null;

            var x = Expression.Parameter(typeof(float), "x");
            var lambda = DynamicExpressionParser.ParseLambda(new[] { x }, null, expression);

            return (Func<float, float>)lambda.Compile();
        }
    }
}
