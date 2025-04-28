using System.Windows.Controls;
using System.Windows.Media;

namespace H.Test.DrawingContextAnimation
{
    public class MyControl4 : Control
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //var effect = AnimationRects.GetDropShadowEffect();
            //BitmapEffectInput effectInput = new BitmapEffectInput() { Input = null }; // null表示应用到整个可视化对象

            //drawingContext.PushEffect(effect, effectInput);
            //drawingContext.DrawRectangle(Brushes.Red, null, new Rect(200, 200, 400, 400));
            //drawingContext.Pop();
        }
    }
}