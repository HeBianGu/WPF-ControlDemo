using System.Windows.Controls;
using System.Windows.Media;

namespace H.Test.DrawingContextAnimation
{
    public class MyControl5 : Control
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //var effect = AnimationRects.GetDropShadowEffect();
            //BitmapEffectInput effectInput = new BitmapEffectInput() { Input = null }; // null表示应用到整个可视化对象

            EllipseGeometry ellipseGeometry = AnimationRects.GetAnimationEllipseGeometry();

            drawingContext.DrawGeometry(Brushes.Red, null, ellipseGeometry);
        }
    }
}