using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace OnlineTerrainGeneratorWebAPI.HeightMapParser
{
    

    public static class HeightMapParser
    {
        public struct HeigthMapParameters
        {
            public string func;
            public string alg;
            public float[] options;
        };
        

        public static HeigthMapParameters JsonParser(string jsonString)
        {
            HeigthMapParameters parameters;
            var jsonObject = JObject.Parse(jsonString);

            string func = (string)jsonObject["func"];
            string alg = (string)jsonObject["alg"];
            JArray options = (JArray)jsonObject["options"];

            float[] optionsArray = options.ToObject<float[]>();

            parameters.func = func;
            parameters.alg = alg;
            parameters.options = optionsArray;

            return parameters;
        }

        public static Func<float, float> HeigthMapFunction(string expression)
        {
            var x = Expression.Parameter(typeof(float), "x");
            var lambda = DynamicExpressionParser.ParseLambda(new[] { x }, null, expression);

            return (Func<float, float>)lambda.Compile();
        }
    }
}
