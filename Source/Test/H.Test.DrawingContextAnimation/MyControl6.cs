using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace H.Test.DrawingContextAnimation
{
    public class MyControl6 : Control
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var path = AnimationRects.GetAnimationPathGeometry();
            drawingContext.DrawGeometry(Brushes.Gray, null, path);
        }
    }
}