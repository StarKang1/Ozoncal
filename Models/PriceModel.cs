using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OzonPriceCalculator.Models
{
    /// <summary>
    /// 价格计算数据模型
    /// </summary>
    public class PriceModel : INotifyPropertyChanged
    {
        private double _productCost;
        private double _productWeight;
        private double _commissionRate = 0.2;
        private double _adRate = 0.4;
        private double _damageRate = 0.1;
        private double _profitRate = 0.2;
        private string _shipFormula = "3 + 0.045 * weight";
        
        private double _calculatedPrice;
        private double _roundedPrice;
        private double _shipCost;
        private double _realCost;

        public double ProductCost
        {
            get => _productCost;
            set { if (_productCost != value) { _productCost = value; OnPropertyChanged(); } }
        }

        public double ProductWeight
        {
            get => _productWeight;
            set { if (_productWeight != value) { _productWeight = value; OnPropertyChanged(); } }
        }

        public double CommissionRate
        {
            get => _commissionRate;
            set { if (_commissionRate != value) { _commissionRate = value; OnPropertyChanged(); } }
        }

        public double AdRate
        {
            get => _adRate;
            set { if (_adRate != value) { _adRate = value; OnPropertyChanged(); } }
        }

        public double DamageRate
        {
            get => _damageRate;
            set { if (_damageRate != value) { _damageRate = value; OnPropertyChanged(); } }
        }

        public double ProfitRate
        {
            get => _profitRate;
            set { if (_profitRate != value) { _profitRate = value; OnPropertyChanged(); } }
        }

        public string ShipFormula
        {
            get => _shipFormula;
            set { if (_shipFormula != value) { _shipFormula = value; OnPropertyChanged(); } }
        }

        public double CalculatedPrice
        {
            get => _calculatedPrice;
            set { if (_calculatedPrice != value) { _calculatedPrice = value; OnPropertyChanged(); } }
        }

        public double RoundedPrice
        {
            get => _roundedPrice;
            set { if (_roundedPrice != value) { _roundedPrice = value; OnPropertyChanged(); } }
        }

        public double ShipCost
        {
            get => _shipCost;
            set { if (_shipCost != value) { _shipCost = value; OnPropertyChanged(); } }
        }

        public double RealCost
        {
            get => _realCost;
            set { if (_realCost != value) { _realCost = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
