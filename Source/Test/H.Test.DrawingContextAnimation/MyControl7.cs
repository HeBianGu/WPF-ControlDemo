using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace H.Test.DrawingContextAnimation
{
    public class MyControl7 : Control
    {
        private DrawingGroup _drawingGroup = new DrawingGroup();
        public MyControl7()
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
                        DrawingGroup drawingGroupItem = new DrawingGroup();
                        using (var contextItem = drawingGroupItem.Open())
                        {
                            contextItem.DrawGeometry(new SolidColorBrush(Colors.Orange) { Opacity = 0.2 }, new Pen(Brushes.Green, 2), new RectangleGeometry(rect));
                            drawingGroup.Children.Add(drawingGroupItem);
                        }
                        drawingGroupItem.Freeze();
                        var frozen = drawingGroup.GetAsFrozen() as DrawingGroup;
                        //Dispatcher.Invoke(() =>
                        //{
                        //    this._drawingGroup = frozen;
                        //    this.InvalidateVisual();
                        //});
                        //Thread.Sleep(10); // 模拟耗时操作,增加延迟主线程操作不那么卡顿

                        Dispatcher.BeginInvoke(DispatcherPriority.Input, () =>
                        {
                            //可以换成Dely方式或者先绘制整体再绘制局部细节分层绘制
                            this._drawingGroup = frozen;
                            this.InvalidateVisual();
                        });

                    }
                }

                //for (int i = 0; i < 50; i++)
                //{
                //    DrawingGroup drawingGroupItem = new DrawingGroup();
                //    using (var contextItem = drawingGroupItem.Open())
                //    {
                //        Rect rect = new Rect(Random.Shared.Next(0, w), Random.Shared.Next(0, h), Random.Shared.Next(w, w * 2), Random.Shared.Next(w, w * 2));
                //        contextItem.DrawGeometry(new SolidColorBrush(Colors.Orange) { Opacity = 0.2 }, null, new RectangleGeometry(rect));
                //        drawingGroup.Children.Add(drawingGroupItem);
                //    }
                //    drawingGroupItem.Freeze();
                //    var frozen = drawingGroup.GetAsFrozen() as DrawingGroup;
                //    Dispatcher.Invoke(() =>
                //    {
                //        this._drawingGroup = frozen;
                //        this.InvalidateVisual();
                //    });
                //    Thread.Sleep(50); // 模拟耗时操作
                //}

                ////// 不需要Dispatcher.Invoke，因为DrawingGroup是Freezable
                ////// 可以冻结后跨线程使用
                //drawingGroup.Freeze();
                //Dispatcher.Invoke(() =>
                //{
                //    //var drawingVisual = new DrawingVisual();
                //    //using (var dc = drawingVisual.RenderOpen())
                //    //{
                //    //    dc.DrawDrawing(drawingGroup);
                //    //}
                //    this._drawingGroup = drawingGroup;
                //    this.InvalidateVisual();
                //});
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