using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Layout.Incremental;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Layout.MDS;
using Microsoft.Msagl.Miscellaneous;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using static H.Test.GraphLayout.MainWindow;
using LineSegment = Microsoft.Msagl.Core.Geometry.Curves.LineSegment;
using P = Microsoft.Msagl.Core.Geometry.Point;
using Point = System.Windows.Point;

namespace H.Test.GraphLayout
{
    public class MyControl : FrameworkElement
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(null, new Pen(Brushes.Orange, 20), new Rect(0, 0, this.RenderSize.Width, this.RenderSize.Height));

            if (gleeGraph == null)
                gleeGraph = this.CreateAndLayoutGraph();
            DrawFromGraph(drawingContext);
        }

        #region - 单独使用Layout功能 -
        GeometryGraph gleeGraph;

        private void DrawFromGraph(DrawingContext graphics)
        {
            SetGraphicsTransform(graphics);
            Pen pen = new Pen(Brushes.Black, 1);
            DrawNodes(pen, graphics);
            DrawEdges(pen, graphics);
            graphics.Pop();

            //graphics.DrawRectangle(null, new Pen(Brushes.Red, 20), new Rect(0, 0, this.RenderSize.Width, this.RenderSize.Height));
        }

        private void DrawEdges(Pen pen, DrawingContext graphics)
        {
            foreach (Edge e in gleeGraph.Edges)
                DrawEdge(e, pen, graphics);
        }

        private void DrawEdge(Edge e, Pen pen, DrawingContext graphics)
        {
            ICurve curve = e.Curve;
            Curve c = curve as Curve;
            if (c != null)
            {
                foreach (ICurve s in c.Segments)
                {
                    var l = s as LineSegment;
                    if (l != null)
                        graphics.DrawLine(pen, MsaglPointToDrawingPoint(l.Start), MsaglPointToDrawingPoint(l.End));
                    CubicBezierSegment cs = s as CubicBezierSegment;
                    //if (cs != null)
                    //    graphics.DrawBezier(pen, MsaglPointToDrawingPoint(cs.B(0)), MsaglPointToDrawingPoint(cs.B(1)), MsaglPointToDrawingPoint(cs.B(2)), MsaglPointToDrawingPoint(cs.B(3)));

                }
                if (e.ArrowheadAtSource)
                    DrawArrow(e, pen, graphics, e.Curve.Start, e.EdgeGeometry.SourceArrowhead.TipPosition);
                if (e.ArrowheadAtTarget)
                    DrawArrow(e, pen, graphics, e.Curve.End, e.EdgeGeometry.TargetArrowhead.TipPosition);
            }
            else
            {
                var l = curve as LineSegment;
                if (l != null)
                    graphics.DrawLine(pen, MsaglPointToDrawingPoint(l.Start), MsaglPointToDrawingPoint(l.End));
            }
        }

        private void DrawArrow(Edge e, Pen pen, DrawingContext graphics, P start, P end)
        {
            Point[] points;
            float arrowAngle = 30;

            P dir = end - start;
            P h = dir;
            dir /= dir.Length;

            P s = new P(-dir.Y, dir.X);

            s *= h.Length * ((float)Math.Tan(arrowAngle * 0.5f * (Math.PI / 180.0)));

            points = new Point[] { MsaglPointToDrawingPoint(start + s), MsaglPointToDrawingPoint(end), MsaglPointToDrawingPoint(start - s) };

            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext ctx = streamGeometry.Open())
            {
                ctx.BeginFigure(points[0], true, true);
                ctx.PolyLineTo(points, true, true);
            }
            graphics.DrawGeometry(pen.Brush, pen, streamGeometry);

            //graphics.FillPolygon(pen.Brush, points);
        }
        private void DrawNodes(Pen pen, DrawingContext graphics)
        {
            foreach (Node n in gleeGraph.Nodes)
                DrawNode(n, pen, graphics);
        }

        private void DrawNode(Node n, Pen pen, DrawingContext graphics)
        {
            ICurve curve = n.BoundaryCurve;
            Ellipse el = curve as Ellipse;
            if (el != null)
            {
                graphics.DrawEllipse(null, pen, MsaglPointToDrawingPoint(el.Center),
                    (float)el.BoundingBox.Width / 2, (float)el.BoundingBox.Height / 2);
            }
            else
            {
                Curve c = curve as Curve;
                foreach (ICurve seg in c.Segments)
                {
                    LineSegment l = seg as LineSegment;
                    if (l != null)
                        graphics.DrawLine(pen, MsaglPointToDrawingPoint(l.Start), MsaglPointToDrawingPoint(l.End));
                }
            }
        }

        private Point MsaglPointToDrawingPoint(P point)
        {
            return new Point((int)point.X, (int)point.Y);
        }

        private void SetGraphicsTransform(DrawingContext drawingContext)
        {
            Rect r = new Rect(0, 0, this.RenderSize.Width, this.RenderSize.Height);
            var gr = this.gleeGraph.BoundingBox;
            if (r.Height > 1 && r.Width > 1)
            {
                float scale = Math.Min((float)r.Width / (float)gr.Width, (float)r.Height / (float)gr.Height);
                float g0 = (float)(gr.Left + gr.Right) / 2;
                float g1 = (float)(gr.Top + gr.Bottom) / 2;

                float c0 = ((float)r.Left + (float)r.Right) / 2;
                float c1 = ((float)r.Top + (float)r.Bottom) / 2;
                float dx = c0 - scale * g0;
                float dy = c1 + scale * g1;
                var matrix = new Matrix(scale, 0, 0, -scale, dx, dy);
                drawingContext.PushTransform(new MatrixTransform(matrix));
            }
        }


        internal GeometryGraph CreateAndLayoutGraph()
        {
            double w = 30;
            double h = 20;
            GeometryGraph graph = new GeometryGraph();
            Node a = new Node(new Ellipse(w, h, new P()), "a");
            Node b = new Node(CurveFactory.CreateRectangle(w, h, new P()), "b");
            Node c = new Node(CurveFactory.CreateRectangle(w, h, new P()), "c");
            Node d = new Node(CurveFactory.CreateRectangle(w, h, new P()), "d");

            graph.Nodes.Add(a);
            graph.Nodes.Add(b);
            graph.Nodes.Add(c);
            graph.Nodes.Add(d);
            Edge e = new Edge(a, b) { Length = 10 };
            graph.Edges.Add(e);
            graph.Edges.Add(new Edge(b, c) { Length = 3 });
            graph.Edges.Add(new Edge(b, d) { Length = 4 });
            //graph.Save("c:\\tmp\\saved.msagl");
            //var settings = new Microsoft.Msagl.Layout.MDS.MdsLayoutSettings();
            //LayoutHelpers.CalculateLayout(graph, settings, null);

            //MdsLayoutSettings GetMdsLayoutSettings()
            //{
            //    var settings = new MdsLayoutSettings
            //    {
            //        EdgeRoutingSettings = { KeepOriginalSpline = true, EdgeRoutingMode = EdgeRoutingMode.StraightLine },
            //        RemoveOverlaps = false
            //    };
            //    return settings;
            //}
            //var settings = GetMdsLayoutSettings();
            //LayoutHelpers.CalculateLayout(graph, settings, null);

            //LayoutHelpers.CalculateLayout(graph, new SugiyamaLayoutSettings(), null);

            //    double level = 1.0;
            //    double edgeLengthMultiplier = 1.0;
            //    var settings = new FastIncrementalLayoutSettings
            //    {
            //        MaxIterations = 10,
            //        MinorIterations = 3,
            //        GravityConstant = 1.0 - level,
            //        RepulsiveForceConstant =
            //Math.Log(edgeLengthMultiplier * level * 500 + Math.E),
            //        InitialStepSize = 0.6
            //    };
            //    foreach (var ed in graph.Edges)
            //        ed.Length *= Math.Log(level * 500 + Math.E);
            //    settings.InitializeLayout(graph, settings.MinConstraintLevel);
            //    LayoutHelpers.CalculateLayout(graph, settings, null);



            var routingSettings = new Microsoft.Msagl.Core.Routing.EdgeRoutingSettings
            {
                BendPenalty = 100,
                EdgeRoutingMode = Microsoft.Msagl.Core.Routing.EdgeRoutingMode.StraightLine
            };
            var settings = new SugiyamaLayoutSettings
            {
                ClusterMargin = 50,
                PackingAspectRatio = 3,
                PackingMethod = Microsoft.Msagl.Core.Layout.PackingMethod.Columns,
                RepetitionCoefficientForOrdering = 0,
                EdgeRoutingSettings = routingSettings,
                NodeSeparation = 50,
                LayerSeparation = 150
            };
            LayoutHelpers.CalculateLayout(graph, settings, null);
            return graph;
        }

        #endregion
    }
}