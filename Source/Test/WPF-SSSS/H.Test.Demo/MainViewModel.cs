using H.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

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