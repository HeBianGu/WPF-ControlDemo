using H.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;

namespace H.Test.Demo
{
    internal class MainViewModel : BindableBase
    {
        private ImageSource _ColorConvertedImagesource;
        /// <summary> 说明  </summary>
        public ImageSource ColorConvertedImageSource
        {
            get { return _ColorConvertedImagesource; }
            set
            {
                _ColorConvertedImagesource = value;
                RaisePropertyChanged();
            }
        }

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


            {

                // 创建源图像
                BitmapImage bitmapImage = new BitmapImage(new Uri("1.jpeg", UriKind.Relative));

                // 定义源颜色上下文 (sRGB)
                ColorContext sourceColorContext = new ColorContext(PixelFormats.Bgra32);

                // 定义目标颜色上下文 (scRGB)
                ColorContext destinationColorContext = new ColorContext(PixelFormats.Gray16);

                // 创建 ColorConvertedBitmap
                ColorConvertedBitmap colorConvertedBitmap = new ColorConvertedBitmap(bitmapImage, sourceColorContext, destinationColorContext, PixelFormats.Pbgra32);

                FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                newFormatedBitmapSource.BeginInit();
                newFormatedBitmapSource.Source = bitmapImage;
                newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray8;
                newFormatedBitmapSource.EndInit();

                // 将 ColorConvertedBitmap 设置为 Image 控件的源
                this.ColorConvertedImageSource = newFormatedBitmapSource;
            }

            {

                //Stream imageStream = new FileStream("1.jpeg", FileMode.Open, FileAccess.Read, FileShare.Read);
                //BitmapSource myBitmapSource = BitmapFrame.Create(imageStream);
                //BitmapFrame myBitmapSourceFrame = (BitmapFrame)myBitmapSource;
                ////ColorContext sourceColorContext = myBitmapSourceFrame.ColorContexts[0];
                //ColorContext sourceColorContext = new ColorContext(PixelFormats.Bgra32);
                //ColorContext destColorContext = new ColorContext(PixelFormats.Prgba128Float);
                //ColorConvertedBitmap ccb = new ColorConvertedBitmap(myBitmapSource, sourceColorContext, destColorContext, PixelFormats.Pbgra32);
                //this.ColorConvertedImageSource = ccb;
                ////imageStream.Close();
            }
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

        //public void SaveAsIcon(RenderTargetBitmap renderTargetBitmap, string filePath)
        //{
        //    PngBitmapEncoder encoder = new PngBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
        //    using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
        //    {
        //        encoder.Save(stream);
        //    }

        //    PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
        //    pngEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        pngEncoder.Save(stream);
        //    }

        //    // 使用 IconBitmapDecoder 读取 PNG 文件
        //    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //    {
        //        IconBitmapDecoder iconDecoder = new IconBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
        //        BitmapFrame iconFrame = iconDecoder.Frames[0];

        //        // 将图标帧保存为 ICO 文件
        //        using (var iconStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            IconBitmapEncoder iconEncoder = new IconBitmapEncoder();
        //            iconEncoder.Frames.Add(iconFrame);
        //            iconEncoder.Save(iconStream);
        //        }
        //    }
        //}
        //}

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