using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Miscellaneous;
using Microsoft.Msagl.WpfGraphControl;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Linq;
using LineSegment = Microsoft.Msagl.Core.Geometry.Curves.LineSegment;
using P = Microsoft.Msagl.Core.Geometry.Point;
using Point = System.Windows.Point;

namespace H.Test.GraphLayout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml https://github.com/microsoft/automatic-graph-layout
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Graph graph = new Graph();
            //graph.AddEdge("Box", "House");
            //graph.AddEdge("House", "InvHouse");
            //graph.AddEdge("InvHouse", "Diamond");
            //graph.AddEdge("Diamond", "Octagon");
            //graph.AddEdge("Octagon", "Hexagon");
            //graph.AddEdge("Hexagon", "2 Circle");
            //graph.AddEdge("2 Circle", "Box");

            //graph.FindNode("Box").Attr.Shape = Shape.Box;
            //graph.FindNode("House").Attr.Shape = Shape.House;
            //graph.FindNode("InvHouse").Attr.Shape = Shape.InvHouse;
            //graph.FindNode("Diamond").Attr.Shape = Shape.Diamond;
            //graph.FindNode("Octagon").Attr.Shape = Shape.Octagon;
            //graph.FindNode("Hexagon").Attr.Shape = Shape.Hexagon;
            //graph.FindNode("2 Circle").Attr.Shape = Shape.DoubleCircle;

            //graph.Attr.LayerDirection = LayerDirection.LR;

            //  Do ：dgml
            //Graph graph = DgmlParser.Parse("fullstring.dgml");

            //SugiyamaLayoutSettings ss = graph.LayoutAlgorithmSettings as SugiyamaLayoutSettings;

            ////  Do ：SameLayer
            //var graph = new Microsoft.Msagl.Drawing.Graph();
            //var sugiyamaSettings = (SugiyamaLayoutSettings)graph.LayoutAlgorithmSettings;
            //sugiyamaSettings.NodeSeparation *= 2;
            //graph.AddEdge("A", "B");
            //graph.AddEdge("A", "C");
            //graph.AddEdge("A", "D");
            ////graph.LayerConstraints.PinNodesToSameLayer(new[] { graph.FindNode("A"), graph.FindNode("B"), graph.FindNode("C") });
            //graph.LayerConstraints.AddSameLayerNeighbors(graph.FindNode("A"), graph.FindNode("B"), graph.FindNode("C"));

            //  Do ：Set
            //graph.Attr.AspectRatio = (double)aspectRatioUpDown.Value;
            //graph.Attr.SimpleStretch = simpleStretchCheckBox.Checked;
            //graph.Attr.MinimalWidth = 5;
            //graph.Attr.MinimalHeight = 5;

            //graphControl.Graph = graph;

            //graphControl.MouseUp += Viewer_MouseUp;
        }
 

        private static void Viewer_MouseUp(object sender, MouseEventArgs e)
        {
            //var gviewer = (AutomaticGraphLayoutControl)sender;
            //var dnode = gviewer.ObjectUnderMouseCursor as Microsoft.Msagl.GraphViewerGdi.DNode;
            //if (dnode == null) return;
            //if (dnode.Node.LabelText == "C")
            //    MessageBox.Show("C is clicked");
        }


        public static class DgmlParser
        {
            public static Microsoft.Msagl.Drawing.Graph Parse(string filename)
            {
                XDocument doc = XDocument.Load(filename);
                var drawingGraph = new Microsoft.Msagl.Drawing.Graph();

                // Parse nodes
                var nodes = doc.Descendants().Where(e => e.Name.LocalName == "Node");
                foreach (var nodeElement in nodes)
                {
                    string id = nodeElement.Attribute("Id")?.Value;
                    if (id == null)
                        continue;

                    string label = nodeElement.Attribute("Label")?.Value ?? id;

                    var node = drawingGraph.AddNode(id);
                    node.LabelText = label;
                }

                // Parse links
                var links = doc.Descendants().Where(e => e.Name.LocalName == "Link");
                foreach (var linkElement in links)
                {
                    string sourceId = linkElement.Attribute("Source")?.Value;
                    string targetId = linkElement.Attribute("Target")?.Value;

                    if (sourceId != null && targetId != null)
                    {
                        var edge = drawingGraph.AddEdge(sourceId, targetId);
                        // Optionally set edge attributes
                        string label = linkElement.Attribute("Label")?.Value;
                        if (!string.IsNullOrEmpty(label))
                        {
                            edge.LabelText = label;
                        }
                    }
                }
                return drawingGraph;
            }
        }
    }
}