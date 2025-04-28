using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace H.Test.DrawingContextAnimation
{
    public class MyControl2 : Control
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);


            drawingContext.PushOpacity(0.5, AnimationRects.GetAnimationOpacityClock());
            drawingContext.DrawRectangle(Brushes.Red, null, new Rect(200, 200, 400, 400));
            drawingContext.Pop();
        }
    }
}