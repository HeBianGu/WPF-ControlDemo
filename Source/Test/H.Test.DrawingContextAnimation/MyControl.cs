using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace H.Test.DrawingContextAnimation
{
    public class MyControl : Control
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            // 在DC中应用动画
            var ss = AnimationBrushes.Flash;
            drawingContext.DrawRectangle(ss, null, new Rect(200, 200, 400, 400));
        }
    }
}