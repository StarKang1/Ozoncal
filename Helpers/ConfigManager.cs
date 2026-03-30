using System;

namespace OzonPriceCalculator.Helpers
{
    /// <summary>
    /// 配置管理器 - 使用内存中的默认配置
    /// </summary>
    public static class ConfigManager
    {
        // 使用内存中的默认配置，避免文件操作
        private static double _commission = 0.2;
        private static double _ads = 0.1;
        private static double _damage = 0.2;
        private static double _profit = 0.3;
        private static string _shipFormula = "3 + 0.045 * weight";

        public static void Initialize()
        {
            // 不需要初始化，直接使用默认值
        }

        public static void Load()
        {
            // 不需要加载，使用默认值
        }

        public static void Save()
        {
            // 不需要保存，使用内存中的值
        }

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
    }
}
