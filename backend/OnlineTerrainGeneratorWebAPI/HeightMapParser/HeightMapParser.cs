using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace OnlineTerrainGeneratorWebAPI.HeightMapParser
{
    /// <summary>
    /// Парсер JSON строки с параметрами карты высот
    /// </summary>
    public static class HeightMapParser
    {
        /// <summary>
        /// Алгоритмы генерации ландшафта
        /// </summary>
        public enum GenerationAlgorithm
        {
            /// <summary>
            /// Diamond-Square
            /// </summary>
            DiamondSquare,
            /// <summary>
            /// Шум Перлина
            /// </summary>
            PerlinNoise,
            /// <summary>
            /// Симплексный шум
            /// </summary>
            SimplexNoise,
            /// <summary>
            /// Неопределен
            /// </summary>
            Unknown
        }
        /// <summary>
        /// Разобранные параметры карты высот, полученные из входной строки JSON
        /// </summary>

        public struct HeigthMapParams
        {
            /// <summary>
            /// Функция шума
            /// </summary>
            public Func<float, float> NoiseExpression;
            /// <summary>
            /// Алгоритм генерации
            /// </summary>
            public GenerationAlgorithm Algorithm;
            /// <summary>
            /// Параметры алгоритма
            /// </summary>
            public float?[] AlgorithmParams;
        };

        /// <summary>
        /// Разбирает входную JSON строку в структуру для передачи параметров карты высот
        /// </summary>
        /// <param name="jsonString">JSON строка</param>
        /// <returns>Структура, содержащая параметры карты высот</returns>       
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
            parameters.Algorithm = alg is not null && Enum.TryParse(alg, out GenerationAlgorithm algorithm) ? algorithm : GenerationAlgorithm.Unknown;
            parameters.AlgorithmParams = optionsArray;

            return parameters;
        }
        /// <summary>
        /// Разбирает математическое выражение в функцию
        /// </summary>
        /// <param name="expression">Математическое выражение</param>
        /// <returns>Функция, полученная в результате разбора входного выражения</returns>
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
