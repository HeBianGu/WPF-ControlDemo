using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace H.Test.DrawingContextAnimation
{
    public class MyControl3 : Control
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            RotateTransform rotateTransform = AnimationRects.GetAnimationRotateTransform();
            drawingContext.PushTransform(rotateTransform);
            drawingContext.DrawRectangle(Brushes.Red, null, new Rect(200, 200, 400, 400));
            drawingContext.Pop();
        }
    }
}