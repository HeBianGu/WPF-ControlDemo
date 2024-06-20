using H.Providers.Mvvm;
using System.Collections.ObjectModel;

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
        }
    }

}