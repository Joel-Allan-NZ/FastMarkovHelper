using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;

namespace WpfApp3
{
    public class AppViewModel : ViewModelBase
    {
        ViewModelBase _contentView = new LoadFileViewModel();

        public ViewModelBase ContentView
        {
            get { return _contentView; }
            set
            {
                if(_contentView != value)
                {
                    _contentView = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void LoadC(object obj) => ContentView = new MainViewModel();



        public AppViewModel()
        {
            ContentView.MarkovLoaded += LoadMarkovView;
            //ContentView = new MainViewModel();
        }

        private void LoadMarkovView(object sender, MarkovLoadedEventArgs e)
        {
            ContentView = new MainViewModel(e.Markov);
        }
    }
}
