using H.Controls.Adorner;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace H.Test.Demo
{
    public class LineGrid : Grid
    {
        protected override void OnRender(DrawingContext dc)
        {
            Pen pen= new Pen(SystemColors.ActiveBorderBrush, 1);
            foreach (RowDefinition item in this.RowDefinitions)
            {
                dc.DrawLine(pen, new Point(0, item.Offset), new Point(this.ActualWidth, item.Offset));
            }
            dc.DrawLine(pen, new Point(0, this.ActualHeight), new Point(this.ActualWidth, this.ActualHeight));

            foreach (ColumnDefinition item in this.ColumnDefinitions)
            {
                dc.DrawLine(pen, new Point(item.Offset, 0), new Point(item.Offset, this.ActualHeight));
            }
            dc.DrawLine(pen, new Point(this.ActualWidth, 0), new Point(this.ActualWidth, this.ActualHeight));
        }
    }

    public class GridLineAdorner : Adorner
    {
        public GridLineAdorner(UIElement adornedElement) : base(adornedElement)
        {
         
        }

        protected override void OnRender(DrawingContext dc)
        {
            Grid grid = this.AdornedElement as Grid;
            if (grid == null)
                return;
            Pen pen = new Pen(SystemColors.HighlightBrush, 1);
            foreach (RowDefinition item in grid.RowDefinitions)
            {
                dc.DrawLine(pen, new Point(0, item.Offset), new Point(this.ActualWidth, item.Offset));
            }
            dc.DrawLine(pen, new Point(0, grid.ActualHeight), new Point(this.ActualWidth, this.ActualHeight));

            foreach (ColumnDefinition item in grid.ColumnDefinitions)
            {
                dc.DrawLine(pen, new Point(item.Offset, 0), new Point(item.Offset, this.ActualHeight));
            }
            dc.DrawLine(pen, new Point(this.ActualWidth, 0), new Point(this.ActualWidth, this.ActualHeight));
           
        }
    }

    public class GridLineAttach
    {
        public static bool GetUse(DependencyObject obj)
        {
            return (bool)obj.GetValue(UseProperty);
        }

        public static void SetUse(DependencyObject obj, bool value)
        {
            obj.SetValue(UseProperty, value);
        }

        public static readonly DependencyProperty UseProperty =
            DependencyProperty.RegisterAttached("Use", typeof(bool), typeof(GridLineAttach), new PropertyMetadata(default(bool), OnUseChanged));

        static public void OnUseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Grid grid = d as Grid;
            if (grid == null)
                return;
            bool n = (bool)e.NewValue;
            grid.Loaded -= Grid_Loaded;
            if (n)
                grid.Loaded += Grid_Loaded;
        }

        private static void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            var controls = grid.Children;
            var count = controls.Count;
            for (int i = 0; i < count; i++)
            {
                var item = controls[i] as FrameworkElement;
                var border = new Border()
                {
                    BorderBrush = SystemColors.ActiveBorderBrush,
                    BorderThickness = new Thickness(1)
                };

                var row = Grid.GetRow(item);
                var column = Grid.GetColumn(item);
                var rowspan = Grid.GetRowSpan(item);
                var columnspan = Grid.GetColumnSpan(item);
                Grid.SetRow(border, row);
                Grid.SetColumn(border, column);
                Grid.SetRowSpan(border, rowspan);
                Grid.SetColumnSpan(border, columnspan);
                grid.Children.Add(border);
            }
        }
    }
}