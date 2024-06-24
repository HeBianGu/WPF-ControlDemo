using H.Providers.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace H.Test.Demo
{
    internal class MainViewModel : NotifyPropertyChangedBase
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

}