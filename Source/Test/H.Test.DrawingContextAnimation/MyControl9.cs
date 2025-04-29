using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace H.Test.DrawingContextAnimation
{

    [Obsolete("未测试成功")]
    public class MyControl9 : Control
    {
        private DrawingGroup _drawingGroup = new DrawingGroup();
        public MyControl9()
        {
            this.SizeChanged += (s, e) =>
            {
                this.Refresh();
            };
            this.Loaded += (s, e) =>
            {
                this.Refresh();
            };
        }

        public void Refresh()
        {
            // 在后台线程构建绘图内容
            int count = 20;
            int w = (int)(this.ActualWidth / 2);
            int h = (int)(this.ActualHeight / 2);
            int stepw = (int)this.ActualWidth / count;
            int steph = (int)this.ActualHeight / count;
            DrawingGroup drawingGroup = new DrawingGroup();
            Parallel.For(0, count, (i) =>
            {
                int cw = stepw * i;
                for (int j = 0; j < count; j++)
                {
                    int ch = steph * j;
                    Rect rect = new Rect(cw, ch, stepw, steph);
                    using (var context = drawingGroup.Append())
                    {
                        context.DrawGeometry(new SolidColorBrush(Colors.Orange) { Opacity = 0.2 }, new Pen(Brushes.Green, 2), new RectangleGeometry(rect));
                    }
                    var frozen = drawingGroup.GetAsFrozen() as DrawingGroup;
                    Dispatcher.BeginInvoke(DispatcherPriority.Input, () =>
                    {
                        this._drawingGroup = frozen;
                        this.InvalidateVisual();
                    });
                }
            });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawDrawing(_drawingGroup);
        }
    }
}