using System.Configuration;
using System.Data;
using System.Windows;

namespace H.Test.Demo1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow mainWindow = new MainWindow();
            LoginWindow loginWindow = new LoginWindow();
            var r = loginWindow.ShowDialog();
            if (r != true)
            {
                this.Shutdown();
                return;
            }
            mainWindow.Show();
        }
    }
}
