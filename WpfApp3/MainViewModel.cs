using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WpfApp3
{
    public class MainViewModel : ViewModelBase
    {
        List<string> _outputOptions;
        string _displayOutput;
        MarkovOptionGenerator _markov;
        string _token;
        string _selectedToken;

        public MarkovOptionGenerator Markov
        {
            get { return _markov; }
            set
            {
                if(_markov != value)
                {
                    _markov = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string DisplayOutput
        {
            get { return _displayOutput; }
            set
            {
                if (_displayOutput != value)
                {
                    _displayOutput = value;
                    RaisePropertyChanged();
                }
            }
        }
        public List<string> OutputOptions
        {
            get { return _outputOptions; }
            set
            {
                if(_outputOptions != value)
                {
                    _outputOptions = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Token
        {
            get { return _token; }
            set
            {
                if (_token != value)
                {
                    _token = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int OptionCount = 9;
        public string SelectedToken
        {
            get { return _selectedToken; }
            set
            {
                if(_selectedToken != value)
                {
                    _selectedToken = value;
                    RaisePropertyChanged();
                }
            }
        }

        //public ICommand OpenFileCommand => new CustomCommand(SpawnOpenFileDialog, x=> true);
        public ICommand GetNextOptionsCommand => new CustomCommand(GetNextOptions, x => _markov != null);
        public ICommand SelectTokenCommand => new CustomCommand(SelectToken, x => SelectedToken != null);
        public ICommand SelectedTokenCommand => new CustomCommand(SelectedTokenAct, x => true);
        public ICommand ClearCommand => new CustomCommand(Clear, x => true);

        private void Clear(object obj)
        {
            //Token = ".";
            GetNextOptions(".");
            DisplayOutput = "";
        }

        private void SelectToken(object obj)
        {
            Token = SelectedToken;
            DisplayOutput += Token;
            DisplayOutput += " ";        
            GetNextOptions(null);
        }

        private void SelectedTokenAct(object obj)
        {
            if(obj != null && obj is string)
            {
                string next = (string)obj;
                if(Token != null && Token.Length == 1)
                {
                    char TokenChar = Token[0];
                    if(Regex.IsMatch(Token, @"([\.\!\?])"))
                    {
                        next = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(next);
                    }
                }
                if (next.Length != 1 && !Regex.IsMatch(next, @"([\.\!\?\,])"))
                {
                    DisplayOutput += " ";
                }              
                DisplayOutput += next;

                GetNextOptions(obj);
            }
        }

        private void GetNextOptions(object obj)
        {
            if(obj != null && obj is string)
            {
                Token = (string)obj;
            }
            IEnumerable<string> nextOptions;
            if( _markov.TryGetOptions(Token, OptionCount, out nextOptions))
            {
                OutputOptions = nextOptions.ToList();
            }
        }

        //private void SpawnOpenFileDialog(object obj)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Text files (*.txt;*.pdf)|*.txt;*.pdf|All files (*.*)|*.*";
        //    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //    openFileDialog.Multiselect = true;

        //    if(openFileDialog.ShowDialog() == true)
        //    {
        //        Markov = new MarkovOptionGenerator(new Markov(openFileDialog.FileNames));
        //        Token = ".";
        //        GetNextOptions(null);
        //        //_Markov.BuildRecord();

        //    }
        //}

        public MainViewModel(List<string> options)
        {
            OutputOptions = options;
        }

        public MainViewModel()
        {
            OutputOptions = new List<string>();
        }

        public MainViewModel(MarkovOptionGenerator markov)
        {
            Markov = markov;
            Token = ".";
            GetNextOptions(null);
        }


    }
}
