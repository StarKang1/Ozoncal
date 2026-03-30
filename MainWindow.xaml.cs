using System;
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
            if (ShippingMethodCombo.SelectedItem is ComboBoxItem selectedItem)
            {
                string method = selectedItem.Content?.ToString() ?? string.Empty;
                if (double.TryParse(WeightInput.Text, out double weight) && double.TryParse(PriceDisplay.Text.Replace("¥ ", ""), out double price))
                {
                    // 转换价格为卢布（假设1人民币=10卢布，实际汇率可能不同）
                    double priceInRubles = price * 10;
                    
                    switch (method)
                    {
                        case "Extra Small":
                            if (weight < 1 || weight > 500)
                            {
                                MessageBox.Show("Extra Small物流方式重量范围为1g至500g，当前重量超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            if (priceInRubles > 1500)
                            {
                                MessageBox.Show("Extra Small物流方式价格限制为最高1500卢布，当前价格超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            break;
                        case "Budget":
                            if (weight < 501 || weight > 25000)
                            {
                                MessageBox.Show("Budget物流方式重量范围为501g至25000g，当前重量超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            if (priceInRubles > 1500)
                            {
                                MessageBox.Show("Budget物流方式价格限制为最高1500卢布，当前价格超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            break;
                        case "Small":
                            if (weight < 1 || weight > 2000)
                            {
                                MessageBox.Show("Small物流方式重量范围为1g至2000g，当前重量超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            if (priceInRubles < 1501 || priceInRubles > 7000)
                            {
                                MessageBox.Show("Small物流方式价格范围为1501至7000卢布，当前价格超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            break;
                        case "Big":
                            if (weight < 2001 || weight > 25000)
                            {
                                MessageBox.Show("Big物流方式重量范围为2001g至25000g，当前重量超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            if (priceInRubles < 1501 || priceInRubles > 7000)
                            {
                                MessageBox.Show("Big物流方式价格范围为1501至7000卢布，当前价格超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            break;
                        case "Premium Small":
                            if (weight < 1 || weight > 5000)
                            {
                                MessageBox.Show("Premium Small物流方式重量范围为1g至5000g，当前重量超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            if (priceInRubles < 7001 || priceInRubles > 250000)
                            {
                                MessageBox.Show("Premium Small物流方式价格范围为7001至250000卢布，当前价格超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            break;
                        case "Premium Big":
                            if (weight < 5001 || weight > 25000)
                            {
                                MessageBox.Show("Premium Big物流方式重量范围为5001g至25000g，当前重量超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            if (priceInRubles < 7001 || priceInRubles > 250000)
                            {
                                MessageBox.Show("Premium Big物流方式价格范围为7001至250000卢布，当前价格超出范围！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            break;
                    }
                }
            }
        }

        private void LoadConfig()
        {
            try
            {
                CommissionInput.Text = (ConfigManager.GetCommission() * 100).ToString();
                AdInput.Text = (ConfigManager.GetAdRate() * 100).ToString();
                DamageInput.Text = (ConfigManager.GetDamageRate() * 100).ToString();
                ProfitInput.Text = (ConfigManager.GetProfitRate() * 100).ToString();
                ShipFormulaInput.Text = ConfigManager.GetShipFormula();
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

                // 计算售价
                double calculatedPrice = PriceService.CalcPrice(
                    cost,
                    weight,
                    _model.CommissionRate,
                    _model.AdRate,
                    _model.DamageRate,
                    _model.ProfitRate,
                    _model.ShipFormula);

                _model.CalculatedPrice = calculatedPrice;

                // 四舍五入
                double roundedPrice = PriceService.RoundPrice(calculatedPrice);
                _model.RoundedPrice = roundedPrice;

                // 计算实际成本（含佣金、广告费和货损）
                double realCost = cost * (1 + _model.DamageRate) + calculatedPrice * (_model.CommissionRate + _model.AdRate);
                _model.RealCost = realCost;

                // 更新UI
                ShipCostDisplay.Text = $"¥ {shipCost:F2}";
                RealCostDisplay.Text = $"¥ {realCost:F2}";
                PriceDisplay.Text = $"¥ {calculatedPrice:F2}";
                RoundedPriceDisplay.Text = $"¥ {roundedPrice:F0}";

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
                if (double.TryParse(CommissionInput.Text, out double commission))
                    ConfigManager.SetCommission(commission / 100.0);
                if (double.TryParse(AdInput.Text, out double ad))
                    ConfigManager.SetAdRate(ad / 100.0);
                if (double.TryParse(DamageInput.Text, out double damage))
                    ConfigManager.SetDamageRate(damage / 100.0);
                if (double.TryParse(ProfitInput.Text, out double profit))
                    ConfigManager.SetProfitRate(profit / 100.0);

                ConfigManager.SetShipFormula(ShipFormulaInput.Text);
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
                string priceText = RoundedPriceDisplay.Text.Replace("¥ ", "").Trim();
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
