using System;
using System.Text.RegularExpressions;

namespace OzonPriceCalculator.Services
{
    /// <summary>
    /// OZON 价格计算服务
    /// </summary>
    public static class PriceService
    {
        /// <summary>
        /// 计算物流成本
        /// </summary>
        public static double CalcShipCost(string formula, double weight)
        {
            try
            {
                // 将公式中的 weight 替换为实际值，然后计算
                var expression = formula.Replace("weight", weight.ToString());
                return EvaluateExpression(expression);
            }
            catch
            {
                // 默认公式
                return 3 + 0.045 * weight;
            }
        }

        /// <summary>
        /// 计算售价（核心公式）
        /// </summary>
        public static double CalcPrice(
            double cost, 
            double weight, 
            double commissionRate,
            double adRate,
            double damageRate,
            double profitRate,
            string shipFormula = "3 + 0.045 * weight")
        {
            // 计算物流成本
            double shipCost = CalcShipCost(shipFormula, weight);

            // 计算实际成本（含货损）
            double realCost = cost * (1 + damageRate);

            // 计算费率总和
            double rateTotal = 1 - commissionRate - adRate - profitRate;

            // 防止分母为0或负数
            if (rateTotal <= 0)
                return 0;

            // 售价公式：(物流成本 + 实际成本) / (1 - 佣金 - 广告 - 利润)
            return (shipCost + realCost) / rateTotal;
        }

        /// <summary>
        /// 四舍五入售价
        /// </summary>
        public static double RoundPrice(double price)
        {
            return Math.Ceiling(price);
        }

        /// <summary>
        /// 简单的表达式计算器（支持 +, -, *, /）
        /// </summary>
        private static double EvaluateExpression(string expression)
        {
            try
            {
                // 移除空格
                expression = expression.Replace(" ", "");

                // 使用 DataTable.Compute 方法计算表达式（简单方案）
                var dt = new System.Data.DataTable();
                var result = dt.Compute(expression, null);
                
                if (result is double d)
                    return d;
                else if (result is int i)
                    return i;
                else
                    return Convert.ToDouble(result);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据重量和价格自动判断物流方式
        /// </summary>
        /// <param name="weight">重量（克）</param>
        /// <param name="priceRub">价格（卢布）</param>
        /// <returns>物流方式</returns>
        public static string GetShippingType(double weight, double priceRub)
        {
            // Extra Small
            if (weight >= 1 && weight <= 500 && priceRub <= 1500)
                return "Extra Small";

            // Budget
            if (weight >= 501 && weight <= 25000 && priceRub <= 1500)
                return "Budget";

            // Small
            if (weight >= 1 && weight <= 2000 && priceRub >= 1501 && priceRub <= 7000)
                return "Small";

            // Big
            if (weight >= 2001 && weight <= 25000 && priceRub >= 1501 && priceRub <= 7000)
                return "Big";

            // Premium Small
            if (weight >= 1 && weight <= 5000 && priceRub >= 7001 && priceRub <= 250000)
                return "Premium Small";

            // Premium Big
            if (weight >= 5001 && weight <= 25000 && priceRub >= 7001 && priceRub <= 250000)
                return "Premium Big";

            return "不符合任何物流类型，请检查重量/售价";
        }
    }
}
