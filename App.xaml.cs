using System;
using System.Windows;
using OzonPriceCalculator.Helpers;

namespace OzonPriceCalculator
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                ConfigManager.Initialize();

                var window = new MainWindow();
                MainWindow = window;
                window.Show();
                window.WindowState = WindowState.Normal;
                window.Activate();
                window.Topmost = true;
                window.Topmost = false;
                window.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"程序启动失败：{ex.Message}", "启动错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ConfigManager.Save();
            base.OnExit(e);
        }
    }
}
