using System.Windows;
using OzonPriceCalculator.Helpers;

namespace OzonPriceCalculator
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // 初始化配置
            ConfigManager.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // 保存配置
            ConfigManager.Save();
            base.OnExit(e);
        }
    }
}
