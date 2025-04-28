using System.Windows.Media;
using System.Windows.Media.Animation;

namespace H.Test.DrawingContextAnimation
{
    public static class AnimationBrushes
    {
        public static SolidColorBrush Flash = GetFlashBrush();

        private static SolidColorBrush GetFlashBrush()
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Orange);

            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = 1;
            doubleAnimation.Duration = TimeSpan.FromSeconds(1);
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            solidColorBrush.BeginAnimation(SolidColorBrush.OpacityProperty, doubleAnimation);
            return solidColorBrush;
        }
    }
}