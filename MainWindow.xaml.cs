using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OzonPriceCalculator.Helpers;
using OzonPriceCalculator.Models;
using OzonPriceCalculator.Services;

namespace OzonPriceCalculator
{
    public partial class MainWindow : Window
    {
        private PriceModel _model = new();

        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();
            LoadConfig();
            // 异步获取汇率
            _ = GetExchangeRateAsync();
        }

        /// <summary>
        /// 异步获取汇率
        /// </summary>
        private async Task GetExchangeRateAsync()
        {
            await ExchangeRateService.GetCnyPerRubAsync();
        }

        private void InitializeUI()
        {
            // 初始化事件处理
            CostInput.TextChanged += (s, e) => Calculate();
            WeightInput.TextChanged += (s, e) => {
                CheckWeightLimit();
                Calculate();
            };
            CommissionInput.TextChanged += (s, e) => Calculate();
            AdInput.TextChanged += (s, e) => Calculate();
            DamageInput.TextChanged += (s, e) => Calculate();
            ProfitInput.TextChanged += (s, e) => Calculate();
            ShipFormulaInput.TextChanged += (s, e) => Calculate();
            ShippingMethodCombo.SelectionChanged += (s, e) => OnShippingMethodChanged();

            // 初始计算
            Calculate();
        }

        private void OnShippingMethodChanged()
        {
            if (ShippingMethodCombo.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                ShipFormulaInput.Text = selectedItem.Tag.ToString();
                CheckWeightLimit();
                Calculate();
            }
        }

        private void CheckWeightLimit()
        {
            // 移除重量提示，仅用于自动选择物流方式
        }

        /// <summary>
        /// 根据推荐物流方式自动选择ComboBox中的对应选项
        /// </summary>
        /// <param name="recommendedShipping">推荐的物流方式</param>
        private void AutoSelectShippingMethod(string recommendedShipping)
        {
            foreach (ComboBoxItem item in ShippingMethodCombo.Items)
            {
                if (item.Content?.ToString() == recommendedShipping)
                {
                    ShippingMethodCombo.SelectedItem = item;
                    break;
                }
            }
        }

        private void LoadConfig()
        {
            try
            {
                CostInput.Text = ConfigManager.GetCost().ToString("F2");
                WeightInput.Text = ConfigManager.GetWeight().ToString();
                CommissionInput.Text = (ConfigManager.GetCommission() * 100).ToString();
                AdInput.Text = (ConfigManager.GetAdRate() * 100).ToString();
                DamageInput.Text = (ConfigManager.GetDamageRate() * 100).ToString();
                ProfitInput.Text = (ConfigManager.GetProfitRate() * 100).ToString();
                ShipFormulaInput.Text = ConfigManager.GetShipFormula();
                
                // 选择物流方式
                string shippingMethod = ConfigManager.GetShippingMethod();
                foreach (ComboBoxItem item in ShippingMethodCombo.Items)
                {
                    if (item.Content?.ToString() == shippingMethod)
                    {
                        ShippingMethodCombo.SelectedItem = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载配置失败: {ex.Message}");
            }
        }

        private void Calculate()
        {
            try
            {
                // 解析输入值
                if (!double.TryParse(CostInput.Text, out double cost) || cost < 0)
                    return;
                if (!double.TryParse(WeightInput.Text, out double weight) || weight < 0)
                    return;
                if (!double.TryParse(CommissionInput.Text, out double commission))
                    return;
                if (!double.TryParse(AdInput.Text, out double adRate))
                    return;
                if (!double.TryParse(DamageInput.Text, out double damage))
                    return;
                if (!double.TryParse(ProfitInput.Text, out double profit))
                    return;

                _model.ProductCost = cost;
                _model.ProductWeight = weight;
                _model.CommissionRate = commission / 100.0;
                _model.AdRate = adRate / 100.0;
                _model.DamageRate = damage / 100.0;
                _model.ProfitRate = profit / 100.0;
                _model.ShipFormula = ShipFormulaInput.Text;

                // 计算物流成本
                double shipCost = PriceService.CalcShipCost(_model.ShipFormula, _model.ProductWeight);
                _model.ShipCost = shipCost;

                // 先算折后成交价，再按促销率反推折前展示价
                double afterDiscountPrice = PriceService.CalcAfterDiscountPrice(
                    cost,
                    weight,
                    _model.CommissionRate,
                    _model.DamageRate,
                    _model.ProfitRate,
                    _model.ShipFormula);

                double beforeDiscountPrice = PriceService.CalcPrice(
                    cost,
                    weight,
                    _model.CommissionRate,
                    _model.AdRate,
                    _model.DamageRate,
                    _model.ProfitRate,
                    _model.ShipFormula);

                _model.CalculatedPrice = afterDiscountPrice;

                // 向上取整后的建议售价
                double roundedAfterDiscountPrice = PriceService.RoundPrice(afterDiscountPrice);
                _model.RoundedPrice = roundedAfterDiscountPrice;

                // 总成本 = 成本 + 货损 + 物流
                double realCost = cost * (1 + _model.DamageRate) + shipCost;
                _model.RealCost = realCost;

                // 转换为卢布价格（按折后成交价）
                double priceRub = ExchangeRateService.ConvertCnyToRub(roundedAfterDiscountPrice);

                // 自动判断物流方式
                string shippingType = PriceService.GetShippingType(weight, priceRub);

                // 自动选择物流方式
                AutoSelectShippingMethod(shippingType);

                // 更新UI
                ShipCostDisplay.Text = $"¥ {shipCost:F2}";
                RealCostDisplay.Text = $"¥ {realCost:F2}";
                PriceDisplay.Text = $"¥ {beforeDiscountPrice:F2}";
                RoundedPriceDisplay.Text = $"¥ {roundedAfterDiscountPrice:F0}";
                RubPriceDisplay.Text = $"{Math.Round(priceRub):F0} RUB";
                RecommendedShippingDisplay.Text = shippingType;

                // 保存配置
                SaveConfig();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"计算错误: {ex.Message}");
            }
        }

        private void SaveConfig()
        {
            try
            {
                if (double.TryParse(CostInput.Text, out double cost))
                    ConfigManager.SetCost(cost);
                if (double.TryParse(WeightInput.Text, out double weight))
                    ConfigManager.SetWeight(weight);
                if (double.TryParse(CommissionInput.Text, out double commission))
                    ConfigManager.SetCommission(commission / 100.0);
                if (double.TryParse(AdInput.Text, out double ad))
                    ConfigManager.SetAdRate(ad / 100.0);
                if (double.TryParse(DamageInput.Text, out double damage))
                    ConfigManager.SetDamageRate(damage / 100.0);
                if (double.TryParse(ProfitInput.Text, out double profit))
                    ConfigManager.SetProfitRate(profit / 100.0);

                ConfigManager.SetShipFormula(ShipFormulaInput.Text);
                if (ShippingMethodCombo.SelectedItem is ComboBoxItem selectedItem)
                {
                    ConfigManager.SetShippingMethod(selectedItem.Content?.ToString() ?? "");
                }

                ConfigManager.Save();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存配置失败: {ex.Message}");
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
            CalculateButton.Content = "✅ 已计算";
            System.Threading.Tasks.Task.Delay(1500).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() => CalculateButton.Content = "🔄 重新计算");
            });
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string priceText = PriceDisplay.Text.Replace("¥ ", "").Trim();
                Clipboard.SetText(priceText);
                
                CopyButton.Content = "✅ 已复制";
                System.Threading.Tasks.Task.Delay(1500).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => CopyButton.Content = "📋 复制售价");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"复制失败: {ex.Message}", "错误");
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void PinButton_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            if (Topmost)
            {
                PinButton.Content = "📌";
                PinButton.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            }
            else
            {
                PinButton.Content = "📌";
                PinButton.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // 窗口拖拽相关变量
        private bool isDragging = false;
        private System.Windows.Point startPoint;

        // 鼠标按下事件 - 开始拖拽
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                isDragging = true;
                startPoint = e.GetPosition(this);
            }
        }

        // 鼠标移动事件 - 执行拖拽
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDragging)
            {
                System.Windows.Point currentPoint = e.GetPosition(this);
                this.Left += currentPoint.X - startPoint.X;
                this.Top += currentPoint.Y - startPoint.Y;
            }
        }

        // 鼠标释放事件 - 结束拖拽
        private void Window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDragging = false;
        }
    }
}
