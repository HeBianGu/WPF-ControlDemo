﻿using H.Data.Test;
using H.Modules.Messages.Dialog;
using System.ComponentModel;
using System.Reflection;
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
using System.Xml.Linq;

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

            var dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(TextBlock.BackgroundProperty, typeof(TextBlock));
            dependencyPropertyDescriptor.AddValueChanged(this.g_dpd, (s, e) =>
            {
                this.g_dpd.Text = this.g_dpd.Background.ToString();
            });

            //在代码中注册控件级全局事件
            this.ic_event.AddHandler(Button.ClickEvent, new RoutedEventHandler(ItemsControl_Button_Click));

            //注册应用程序级全局事件
            EventManager.RegisterClassHandler(typeof(Button), Button.ClickEvent, new RoutedEventHandler(ItemsControl_Button_Click));
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                this.tb_doubleclick.Text += "双击" + Environment.NewLine;
            if (e.ClickCount == 3)
                this.tb_doubleclick.Text += "三连击" + Environment.NewLine;
        }

        private void btn_async_Click(object sender, RoutedEventArgs e)
        {
            IsAsyncWindow isAsyncWindo = new IsAsyncWindow();
            isAsyncWindo.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BeiginInvokeWindow beiginInvokeWindow = new BeiginInvokeWindow();
            beiginInvokeWindow.Show();
        }

        private void Button_AdornerDialog_Click(object sender, RoutedEventArgs e)
        {
            AdornerDialog.ShowPresenter("我是AdornerDialog");
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Grid_Loaded");
            //VisualBrush visualBrush = new VisualBrush();
            //ContentPresenter contentPresenter = new ContentPresenter();
            //DataTemplateKey dataTemplateKey = new DataTemplateKey(typeof(Student));
            //contentPresenter.ContentTemplate=Application.Current.FindResource(dataTemplateKey) as DataTemplate;
            //contentPresenter.Content = new Student();
            //visualBrush.Visual = contentPresenter;
            //contentPresenter.InvalidateMeasure();
            //contentPresenter.InvalidateArrange();
            //this.Background = visualBrush;
        }

        private void GridLine_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            GridLineAdorner gridLineAdorner = new GridLineAdorner(grid);
            var layer = AdornerLayer.GetAdornerLayer(grid);
            layer.Add(gridLineAdorner);
        }

        private void ItemsControl_Button_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button)
                MessageBox.Show(button.Content?.ToString());
        }

        private void ic_event_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void ic_event_GotMouseCapture(object sender, MouseEventArgs e)
        {

        }
    }

}