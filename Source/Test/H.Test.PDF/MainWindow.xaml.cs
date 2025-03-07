using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection.PortableExecutable;
//using PdfSharp.Pdf.IO;
//using PdfSharp.Pdf;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig;
using System.Diagnostics;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Content;
using static UglyToad.PdfPig.Core.PdfSubpath;
using Line = UglyToad.PdfPig.Core.PdfSubpath.Line;
using System.Collections.ObjectModel;
using H.Mvvm;
using Microsoft.Win32;
using System.IO;

namespace H.Test.PDF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //public string ExtractTextFromPdf(string filePath)
        //{
        //    string pdfPath = "path/to/your/pdf/file.pdf";
        //    using (PdfReader reader = new PdfReader(pdfPath))
        //    {
        //        using (PdfDocument pdfDoc = new PdfDocument(reader))
        //        {
        //            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
        //            Console.WriteLine("Title: " + info.GetTitle());
        //            Console.WriteLine("Author: " + info.GetAuthor());
        //            Console.WriteLine("Subject: " + info.GetSubject());
        //            Console.WriteLine("Keywords: " + info.GetKeywords());
        //            Console.WriteLine("Creator: " + info.GetCreator());
        //            Console.WriteLine("Producer: " + info.GetProducer());
        //            Console.WriteLine("Creation Date: " + info.GetMoreInfo("CreationDate"));
        //            Console.WriteLine("Modification Date: " + info.GetMoreInfo("ModDate"));
        //        }
        //    }
        //}


        //public void Method()
        //{
        //    using (PdfDocument document = PdfReader.Open(pdfPath))
        //    {
        //        Console.WriteLine("Title: " + document.Info.Title);
        //        Console.WriteLine("Author: " + document.Info.Author);
        //        Console.WriteLine("Subject: " + document.Info.Subject);
        //        Console.WriteLine("Keywords: " + document.Info.Keywords);
        //        Console.WriteLine("Creator: " + document.Info.Creator);
        //        Console.WriteLine("Producer: " + document.Info.Producer);
        //        Console.WriteLine("Creation Date: " + document.Info.CreationDate);
        //        Console.WriteLine("Modification Date: " + document.Info.ModificationDate);


        //        // 读取每个页面及其元数据
        //        for (int i = 0; i < document.PageCount; i++)
        //        {
        //            PdfPage page = document.Pages[i];
        //            Console.WriteLine($"Page {i + 1}:");
        //            Console.WriteLine($"  Width: {page.Width}");
        //            Console.WriteLine($"  Height: {page.Height}");
        //            Console.WriteLine($"  Rotate: {page.Rotate}");
        //            Console.WriteLine($"  MediaBox: {page.MediaBox}");
        //            Console.WriteLine($"  CropBox: {page.CropBox}");
        //            Console.WriteLine($"  BleedBox: {page.BleedBox}");
        //            Console.WriteLine($"  TrimBox: {page.TrimBox}");
        //            Console.WriteLine($"  ArtBox: {page.ArtBox}");
        //        }
        //    }
        //}


    }

    public class MainViewModel : BindableBase
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                RaisePropertyChanged();
            }
        }


        public RelayCommand OpenFileCommand => new RelayCommand(x =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory; //设置初始路径
            openFileDialog.Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*"; //设置“另存为文件类型”或“文件类型”框中出现的选择内容
            openFileDialog.FilterIndex = 1; //设置默认显示文件类型为Csv文件(*.csv)|*.csv
            openFileDialog.Title = "打开文件"; //获取或设置文件对话框标题
            openFileDialog.RestoreDirectory = true; //设置对话框是否记忆上次打开的目录
            openFileDialog.Multiselect = false;//设置多选
            if (openFileDialog.ShowDialog() != true) return;

            this.FilePath = openFileDialog.FileName;
        });
    }

    public class PDFViewer : FrameworkElement
    {
        PdfDocument document;
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(PDFViewer), new FrameworkPropertyMetadata(default(string), (d, e) =>
            {
                PDFViewer control = d as PDFViewer;

                if (control == null) return;

                if (e.OldValue is string o)
                {

                }

                if (e.NewValue is string n)
                {

                }
                control.LoadFile();
                control.InvalidateVisual();
            }));


        public void Close()
        {
            this.document?.Dispose();
        }


        public void LoadFile()
        {
            string pdfPath = this.FilePath;
            if (!File.Exists(pdfPath))
                return;
            document = PdfDocument.Open(pdfPath);

            this.PageIndexs = new ObservableCollection<int>(Enumerable.Range(1, document.NumberOfPages));
            //this.PageNames = new ObservableCollection<string>(document.GetPages().Select(p => p.Text));
            this.PageIndex = 1;
            {
                //// 读取文档元数据
                //Debug.WriteLine("Title: " + document.Information.Title);
                //Debug.WriteLine("Author: " + document.Information.Author);
                //Debug.WriteLine("Subject: " + document.Information.Subject);
                //Debug.WriteLine("Keywords: " + document.Information.Keywords);
                //Debug.WriteLine("Creator: " + document.Information.Creator);
                //Debug.WriteLine("Producer: " + document.Information.Producer);
                //Debug.WriteLine("Creation Date: " + document.Information.CreationDate);
                ////Console.WriteLine("Modification Date: " + document.Information.ModificationDate);

                //// 读取每个页面及其内容
                //foreach (UglyToad.PdfPig.Content.Page page in document.GetPages())
                //{
                //    Debug.WriteLine($"Page {page.Number}:");
                //    //// 读取页面上的文字
                //    //foreach (var word in page.GetWords())
                //    //{
                //    //    Debug.WriteLine($"  Text: {word.Text}");
                //    //}

                //    var wordCount = page.GetWords().Count();
                //    var pathCount = page.ExperimentalAccess.Paths.Count;
                //    System.Diagnostics.Debug.WriteLine("GetWords:" + wordCount);
                //    System.Diagnostics.Debug.WriteLine("Paths:" + pathCount);

                //    //// 读取页面上的线条
                //    //foreach (PdfPath path in page.ExperimentalAccess.Paths)
                //    //{
                //    //    //foreach (PdfSubpath item in path)
                //    //    //{
                //    //    //    foreach (var command in item.Commands)
                //    //    //    {
                //    //    //        Debug.WriteLine(command);

                //    //    //        //if (command is PdfPath.LineTo lineTo)
                //    //    //        //{
                //    //    //        //    Console.WriteLine($"  Line: Start({lineTo.From.X}, {lineTo.From.Y}) - End({lineTo.To.X}, {lineTo.To.Y})");
                //    //    //        //}
                //    //    //    }
                //    //    //}

                //    //}
                //}
            }
        }


        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(PDFViewer), new FrameworkPropertyMetadata(default(int), (d, e) =>
            {
                PDFViewer control = d as PDFViewer;

                if (control == null) return;

                if (e.OldValue is int o)
                {

                }

                if (e.NewValue is int n)
                {

                }
                control.LoadSize();
                control.InvalidateVisual();
            }));


        public void LoadSize()
        {
            //var pages = document.GetPages().FirstOrDefault(x => x.Number == this.PageIndex + 1);
            //if (this.PageIndex < 1 || this.PageIndex > pages.Count)
            //    return;
            var page = document.GetPage(this.PageIndex);
            var rect = new Rect(0, 0, page.Width, page.Height);
            this.Width = page.Width;
            this.Height = page.Height;
        }


        public ObservableCollection<string> PageNames
        {
            get { return (ObservableCollection<string>)GetValue(PageNamesProperty); }
            set { SetValue(PageNamesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageNamesProperty =
            DependencyProperty.Register("PageNames", typeof(ObservableCollection<string>), typeof(PDFViewer), new FrameworkPropertyMetadata(default(ObservableCollection<string>), (d, e) =>
            {
                PDFViewer control = d as PDFViewer;

                if (control == null) return;

                if (e.OldValue is ObservableCollection<string> o)
                {

                }

                if (e.NewValue is ObservableCollection<string> n)
                {

                }

            }));


        public ObservableCollection<int> PageIndexs
        {
            get { return (ObservableCollection<int>)GetValue(PageIndexsProperty); }
            set { SetValue(PageIndexsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageIndexsProperty =
            DependencyProperty.Register("PageIndexs", typeof(ObservableCollection<int>), typeof(PDFViewer), new FrameworkPropertyMetadata(default(ObservableCollection<int>), (d, e) =>
            {


            }));



        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (document == null)
                return;

            //var pages = document.GetPages().ToList();
            //if (this.PageIndex < 1 || this.PageIndex > pages.Count)
            //    return;
            var page = document.GetPage(this.PageIndex);

            //var page = pages[this.PageIndex - 1];

            foreach (Word item in page.GetWords())
            {
                var p = item.BoundingBox.TopLeft;
                FormattedText formattedText = new FormattedText(item.Text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(item.FontName), item.BoundingBox.Height + 0, Brushes.Black);
                drawingContext.DrawText(formattedText, new Point(p.X, page.Height - p.Y));
            }

            foreach (PdfPath path in page.ExperimentalAccess.Paths)
            {
                foreach (PdfSubpath item in path)
                {
                    foreach (IPathCommand command in item.Commands)
                    {
                        if (command is Line line)
                        {
                            drawingContext.DrawLine(new Pen(Brushes.Red, 1), new Point(line.From.X, page.Height - line.From.Y), new Point(line.To.X, page.Height - line.To.Y));
                        }
                        if (command is BezierCurve bezier)
                        {
                            drawingContext.DrawLine(new Pen(Brushes.Red, 1), new Point(bezier.StartPoint.X, page.Height - bezier.StartPoint.Y), new Point(bezier.EndPoint.X, page.Height - bezier.EndPoint.Y));
                        }
                        //if (command is Move move)
                        //{
                        //    drawingContext.DrawLine(new Pen(Brushes.Red, 1), new Point(line.From.X, line.From.Y), new Point(line.To.X, line.To.Y));
                        //}

                        //if (command is PdfPath.LineTo lineTo)
                        //{
                        //    Console.WriteLine($"  Line: Start({lineTo.From.X}, {lineTo.From.Y}) - End({lineTo.To.X}, {lineTo.To.Y})");
                        //}
                    }
                }

            }

            var rect = new Rect(0, 0, page.Width, page.Height);
            drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Blue, 1), rect);

        }
    }
}