using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfApp3
{
    public class LoadFileViewModel : ViewModelBase
    {
        MarkovOptionGenerator _markov;
        string _buttonText = "Load Source File(s)";
        //string _token;
        bool _loading = false;
        //stri
        public MarkovOptionGenerator Markov
        {
            get => _markov;
            set
            {
                if(value != _markov)
                {
                    _markov = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool Loading
        {
            get => _loading;
            set
            {
                if(_loading != value)
                {
                    _loading = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if(_buttonText != value)
                {
                    _buttonText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand OpenFileCommand => new CustomCommand(SpawnOpenFileDialog, x => !Loading);

        private void SpawnOpenFileDialog(object obj)
        {
            //Loading = true;
            Task.Run(() => Load()).ContinueWith(task => { Loading = false; });
            
        }

        private void Load()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text files (*.txt;*.pdf)|*.txt;*.pdf|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    Loading = true;
                    ButtonText = "Loading...";
                    Markov = new MarkovOptionGenerator(new Markov(openFileDialog.FileNames));
                    MarkovLoaded(this, new MarkovLoadedEventArgs(Markov));
                }
            }
            catch
            {
                //retry
                Load();
            }
        }
    }
}
