using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace OnlineTerrainGeneratorWebAPI.HeightMapParser
{
    public static class HeightMapParser
    {
        public enum GenerationAlgoritm
        {
            DiamondSquare,
            PerlinNoise,
            SimplexNoise
        }

        public struct HeigthMapParams
        {
            public Func<float, float> NoiseExpression;
            public GenerationAlgoritm Algorithm;
            public float[] AlgorithmParams;
        };
        
        public static HeigthMapParams JsonParser(string jsonString)
        {
            HeigthMapParams parameters;
            var jsonObject = JObject.Parse(jsonString);

            var func = (string)jsonObject["func"];
            var alg = (string)jsonObject["alg"];
            var options = (JArray)jsonObject["options"];

            var optionsArray = options.ToObject<float[]>();

            parameters.NoiseExpression = HeigthMapFunction(func);
            parameters.Algorithm = (GenerationAlgoritm)Enum.Parse(typeof(GenerationAlgoritm), alg);
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
