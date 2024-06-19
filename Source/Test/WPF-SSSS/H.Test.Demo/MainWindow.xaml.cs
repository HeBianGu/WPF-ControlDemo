using H.Modules.Messages.Dialog;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace H.Test.Demo
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

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                this.tb_doubleclick.Text += "双击" + Environment.NewLine;
            if (e.ClickCount == 3)
                this.tb_doubleclick.Text += "三连击" + Environment.NewLine;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BeiginInvokeWindow beiginInvokeWindow=new BeiginInvokeWindow();
            beiginInvokeWindow.Show();
        }

        private void Button_AdornerDialog_Click(object sender, RoutedEventArgs e)
        {
            AdornerDialog.ShowPresenter("我是AdornerDialog");
        }
    }
}