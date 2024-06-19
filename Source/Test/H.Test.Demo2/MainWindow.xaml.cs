using H.Modules.Messages.Dialog;
using H.Providers.Ioc;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace H.Test.Demo2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_AdornerDialog_Click(object sender, RoutedEventArgs e)
        {
            var r = await AdornerDialog.ShowPresenter("我是AdornerDialog", x =>
            {
                x.DialogButton = DialogButton.SumitAndCancel;
                x.Height = 100;
                x.VerticalContentAlignment = VerticalAlignment.Top;
                x.Padding = new Thickness(20);
                });
            if (r == true)
                System.Diagnostics.Debug.WriteLine("点击了确定");
            if (r == null)
                System.Diagnostics.Debug.WriteLine("点击了关闭按钮");
            if (r == true)
                System.Diagnostics.Debug.WriteLine("点击了取消");
        }
    }
}