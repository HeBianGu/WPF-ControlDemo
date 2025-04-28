using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace H.Test.DrawingContextAnimation
{
    public class AnimationRects
    {
        public static AnimationClock GetRectAnimationClock()
        {
            RectAnimation doubleAnimation = new RectAnimation();
            doubleAnimation.From = new Rect(0, 0, 100, 200);
            doubleAnimation.To = new Rect(100, 200, 500, 300);
            doubleAnimation.Duration = TimeSpan.FromSeconds(3);
            doubleAnimation.AutoReverse = true;
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            return doubleAnimation.CreateClock();

        }

        public static AnimationClock GetAnimationOpacityClock()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0.0;
            doubleAnimation.To = 1;
            doubleAnimation.Duration = TimeSpan.FromSeconds(2);
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            return doubleAnimation.CreateClock();
        }


        public static RotateTransform GetAnimationRotateTransform()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0.0;
            doubleAnimation.To = 360;
            doubleAnimation.Duration = TimeSpan.FromSeconds(2);
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = 90;
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
            return rotateTransform;
        }


        public static DropShadowEffect GetAnimationDropShadowEffect()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0.0;
            doubleAnimation.To = 360;
            doubleAnimation.Duration = TimeSpan.FromSeconds(2);
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.ShadowDepth = 4;
            dropShadowEffect.BeginAnimation(DropShadowEffect.ShadowDepthProperty, doubleAnimation);
            return dropShadowEffect;
        }


        public static EllipseGeometry GetAnimationEllipseGeometry()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 10.0;
            doubleAnimation.To = 360;
            doubleAnimation.Duration = TimeSpan.FromSeconds(2);
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.RadiusY = 100;
            ellipseGeometry.RadiusX = 300;
            ellipseGeometry.Center = new Point(400, 400);
            ellipseGeometry.BeginAnimation(EllipseGeometry.RadiusXProperty, doubleAnimation);
            return ellipseGeometry;
        }


        public static PathGeometry GetAnimationPathGeometry()
        {
            // 创建起始和结束几何图形
            var geometry = Geometry.Parse("M50,100 75,50 125,50 150,100 225,150 250,100 250,200 C225,250 175,250 150,200 S75,150 50,200Z");

            var path = PathGeometry.CreateFromGeometry(geometry);
            // 创建动画
            var duration = TimeSpan.FromSeconds(2);

            foreach (var figure in path.Figures)
            {
                foreach (var segment in figure.Segments)
                {
                    if(segment is BezierSegment bezierSegment)
                    {
                        {
                            var anim1 = new PointAnimation(new Point(), bezierSegment.Point1, duration);
                            bezierSegment.BeginAnimation(BezierSegment.Point1Property, anim1);
                        }
                        {
                            var anim1 = new PointAnimation(new Point(), bezierSegment.Point2, duration);
                            bezierSegment.BeginAnimation(BezierSegment.Point2Property, anim1);
                        }
                        {
                            var anim1 = new PointAnimation(new Point(), bezierSegment.Point3, duration);
                            bezierSegment.BeginAnimation(BezierSegment.Point3Property, anim1);
                        }
                    }

                    if (segment is LineSegment lineSegment)
                    {
                        {
                            var anim1 = new PointAnimation(new Point(), lineSegment.Point, duration);
                            lineSegment.BeginAnimation(BezierSegment.Point1Property, anim1);
                        }
                    }

                    //if (segment is PolyLineSegment polyLineSegment)
                    //{
                    //    {
                    //        var anim1 = new PointAnimation(new Point(), lineSegment.Point, duration);
                    //        lineSegment.BeginAnimation(BezierSegment.Point1Property, anim1);
                    //    }
                    //}

                }
            }
            return path;
        }
    }
}