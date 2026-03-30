using System;

namespace OzonPriceCalculator.Helpers
{
    /// <summary>
    /// 配置管理器 - 使用内存中的默认配置
    /// </summary>
    public static class ConfigManager
    {
        // 配置数据
        private static double _cost = 20.00;
        private static double _weight = 200;
        private static string _shippingMethod = "Extra Small";
        private static double _commission = 0.2;
        private static double _ads = 0.05;
        private static double _damage = 0.1;
        private static double _profit = 0.3;
        private static string _shipFormula = "3 + 0.045 * weight";

        static ConfigManager()
        {
            // 不需要初始化
        }

        public static void Initialize()
        {
            // 不需要初始化
        }

        public static void Load()
        {
            // 不需要加载，使用默认值
        }

        public static void Save()
        {
            // 不需要保存，使用内存中的值
        }

        public static double GetCost() => _cost;
        public static void SetCost(double value) { _cost = value; }

        public static double GetWeight() => _weight;
        public static void SetWeight(double value) { _weight = value; }

        public static string GetShippingMethod() => _shippingMethod;
        public static void SetShippingMethod(string value) { _shippingMethod = value; }

        public static double GetCommission() => _commission;
        public static void SetCommission(double value) { _commission = value; }

        public static double GetAdRate() => _ads;
        public static void SetAdRate(double value) { _ads = value; }

        public static double GetDamageRate() => _damage;
        public static void SetDamageRate(double value) { _damage = value; }

        public static double GetProfitRate() => _profit;
        public static void SetProfitRate(double value) { _profit = value; }

        public static string GetShipFormula() => _shipFormula;
        public static void SetShipFormula(string value) { _shipFormula = value; }

        // 配置数据类
        private class ConfigData
        {
            public double Cost { get; set; }
            public double Weight { get; set; }
            public string? ShippingMethod { get; set; }
            public double Commission { get; set; }
            public double Ads { get; set; }
            public double Damage { get; set; }
            public double Profit { get; set; }
            public string? ShipFormula { get; set; }
        }
    }
}
