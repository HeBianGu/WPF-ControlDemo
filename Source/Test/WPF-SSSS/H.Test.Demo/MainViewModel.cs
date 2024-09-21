using H.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace H.Test.Demo
{
    internal class MainViewModel : BindableBase
    {
        private ObservableCollection<string> _collection = new ObservableCollection<string>();
        public ObservableCollection<string> Collection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                RaisePropertyChanged("Collection");
            }
        }

        public MainViewModel()
        {
            var source = Enumerable.Range(0, 10000).Select(x => x.ToString()).ToObservable();
            this.Collection = source;

            this.MyGenericTypes.Add(new MyGenericClass<Int32>());
            this.MyGenericTypes.Add(new MyGenericClass<string>());

            this.TaskCompletionSourceTests.Add(new TaskCompletionSourceTest());

            this.DrawingContextWithWriteableBitmapExample();
        }


        private ObservableCollection<TaskCompletionSourceTest> _TaskCompletionSourceTests = new ObservableCollection<TaskCompletionSourceTest>();
        /// <summary> 说明  </summary>
        public ObservableCollection<TaskCompletionSourceTest> TaskCompletionSourceTests
        {
            get { return _TaskCompletionSourceTests; }
            set
            {
                _TaskCompletionSourceTests = value;
                RaisePropertyChanged("TaskCompletionSourceTests");
            }
        }


        private ObservableCollection<object> _myGenericTypes = new ObservableCollection<object>();
        /// <summary> 说明  </summary>
        public ObservableCollection<object> MyGenericTypes
        {
            get { return _myGenericTypes; }
            set
            {
                _myGenericTypes = value;
                RaisePropertyChanged("MyGenericTypes");
            }
        }


        private ImageSource _ImageSource;
        /// <summary> 说明  </summary>
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set
            {
                _ImageSource = value;
                RaisePropertyChanged();
            }
        }


        public void DrawingContextWithWriteableBitmapExample()
        {
            int width = 200;
            int height = 200;
            WriteableBitmap writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);

            // 使用 DrawingVisual 和 DrawingContext 绘制图形
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(Brushes.Blue, null, new Rect(50, 50, 100, 100));
                drawingContext.DrawText(
                    new FormattedText(
                        "Hello, WPF!",
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Verdana"),
                        32,
                        Brushes.White),
                    new Point(60, 60));

                drawingContext.DrawDrawing(new GeometryDrawing(Brushes.Red, new Pen(Brushes.Black, 1), new RectangleGeometry(new Rect(10, 10, 50, 50))));
            }

            // 将 DrawingVisual 渲染到 WriteableBitmap 中
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(drawingVisual);

            // 将 RenderTargetBitmap 的内容复制到 WriteableBitmap
            writeableBitmap.Lock();
            renderTargetBitmap.CopyPixels(new Int32Rect(0, 0, width, height), writeableBitmap.BackBuffer, writeableBitmap.BackBufferStride * height, writeableBitmap.BackBufferStride);
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            writeableBitmap.Unlock();

            // 将 WriteableBitmap 绑定到 Image 控件以显示图像
            //Image image = new Image();
            //image.Source = writeableBitmap;

            //this.Content = image;
            //this.Width = 250;
            //this.Height = 250;

            this.ImageSource = writeableBitmap;

            // 保存为 PNG 文件
            SaveAsPng(renderTargetBitmap, "output.png");

            // 保存为 JPEG 文件
            SaveAsJpeg(renderTargetBitmap, "output.jpg");

            // 保存为 BMP 文件
            SaveAsBmp(renderTargetBitmap, "output.bmp");
        }

        public void SaveAsPng(RenderTargetBitmap renderTargetBitmap, string filePath)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        public void SaveAsJpeg(RenderTargetBitmap renderTargetBitmap, string filePath)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        public void SaveAsBmp(RenderTargetBitmap renderTargetBitmap, string filePath)
        {
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(stream);
            }
        }
    }

    public class TaskCompletionSourceTest : Bindable
    {

        private string _message;
        /// <summary> 说明  </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }

        TaskCompletionSource<bool> _taskCompletionSource;
        CancellationTokenSource _tokenSource;
        public RelayCommand SumitCommand => new RelayCommand((s, e) =>
        {
            this.Message += Environment.NewLine + "点击提交";
            this.Result = true;
            _taskCompletionSource.SetResult(true);
        });

        public bool Result { get; set; }

        public RelayCommand CancelCommand => new RelayCommand((s, e) =>
        {
            this.Message += Environment.NewLine + "点击取消";
            _tokenSource.Cancel();
            _taskCompletionSource.SetResult(false);
            this.Result = false;
            //// 会报错
            //_taskCompletionSource.SetCanceled();
        });

        public RelayCommand RunAsyncCommand => new RelayCommand(async (s, e) =>
        {
            this.Message += Environment.NewLine + "开始任务";
            _taskCompletionSource = new TaskCompletionSource<bool>();
            _tokenSource = new CancellationTokenSource();
            await this.RunAsync(_tokenSource.Token);
            this.Message += Environment.NewLine + "执行完成,结果：" + this.Result;
        });

        public RelayCommand ExceptionCommand => new RelayCommand((s, e) =>
        {
            this.Message += Environment.NewLine + "点击异常";
            _taskCompletionSource.SetException(new Exception());
        });

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 10; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                this.Message += Environment.NewLine + "步骤：" + i;
                await Task.Delay(1000);
            }
            await _taskCompletionSource.Task;
        }
    }

}