using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace H.Test.DrawingContextAnimation
{
    public class MyControl8 : Control
    {
        private DrawingGroup _drawingGroup = new DrawingGroup();
        public MyControl8()
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
            Task.Run(() =>
            {
                int count = 20;
                DrawingGroup drawingGroup = new DrawingGroup();
                int w = (int)(this.ActualWidth / 2);
                int h = (int)(this.ActualHeight / 2);
                int stepw = (int)this.ActualWidth / count;
                int steph = (int)this.ActualHeight / count;
                for (int i = 0; i < count; i++)
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
                            //可以换成Dely方式或者先绘制整体再绘制局部细节分层绘制
                            this._drawingGroup = frozen;
                            this.InvalidateVisual();
                        });

                    }
                }
            });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //drawingContext.DrawRectangle(null, new Pen(Brushes.Orange, 3), new Rect(Random.Shared.NextDouble() * this.ActualWidth, 0, this.ActualWidth, this.ActualHeight));
            drawingContext.DrawDrawing(_drawingGroup);
        }
    }
}