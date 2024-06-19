using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace H.Test.Demo
{
    /// <summary>
    /// BeiginInvokeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BeiginInvokeWindow : Window
    {
        public BeiginInvokeWindow()
        {
            InitializeComponent();
            this.Loaded += this.BeiginInvokeWindow_Loaded;
        }

        private void BeiginInvokeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var data = Enumerable.Range(0, 10 * 1000);
            ObservableCollection<int> beginInvokeDatas = new ObservableCollection<int>();
            this.lb.ItemsSource = beginInvokeDatas;
            foreach (var item in data)
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    beginInvokeDatas.Insert(0, item);
                }, DispatcherPriority.ApplicationIdle);
            }
        }
    }
}
