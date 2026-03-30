using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace OzonPriceCalculator.Services
{
    /// <summary>
    /// 汇率服务
    /// </summary>
    public static class ExchangeRateService
    {
        private static double _cnyPerRub = 0.0831; // 默认汇率，1 RUB = 0.0831 CNY
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// 获取卢布对人民币的汇率
        /// </summary>
        /// <returns>1 RUB 等于多少 CNY</returns>
        public static async Task<double> GetCnyPerRubAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://api.exchangerate.host/latest?base=RUB&symbols=CNY");
                var data = JsonSerializer.Deserialize<ExchangeRateResponse>(response);
                if (data != null && data.success && data.rates != null && data.rates.ContainsKey("CNY"))
                {
                    _cnyPerRub = data.rates["CNY"];
                }
            }
            catch
            {
                // 如果API调用失败，使用默认汇率
            }
            return _cnyPerRub;
        }

        /// <summary>
        /// 获取当前汇率
        /// </summary>
        /// <returns>1 RUB 等于多少 CNY</returns>
        public static double GetCurrentCnyPerRub()
        {
            return _cnyPerRub;
        }

        /// <summary>
        /// 将人民币转换为卢布
        /// </summary>
        /// <param name="cnyPrice">人民币价格</param>
        /// <returns>卢布价格</returns>
        public static double ConvertCnyToRub(double cnyPrice)
        {
            return cnyPrice / _cnyPerRub;
        }

        /// <summary>
        /// 汇率响应模型
        /// </summary>
        private class ExchangeRateResponse
        {
            public bool success { get; set; }
            public string? @base { get; set; }
            public System.Collections.Generic.Dictionary<string, double>? rates { get; set; }
        }
    }
}