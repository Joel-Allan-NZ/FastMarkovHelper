using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class Markov
    {
        public Dictionary<string, MarkovDatum> Tokens { get; private set; }

        public Markov(string fileLocation)
        {
            Tokens = new Dictionary<string, MarkovDatum>();
            _fileLocations = new List<string>() { fileLocation };
            BuildRecord();
        }

        public Markov(IEnumerable<string> fileLocations)
        {
            Tokens = new Dictionary<string, MarkovDatum>();
            _fileLocations = fileLocations;
            BuildRecord();

        }

        public void BuildRecord()
        {
            string CurrentTokenName = string.Empty;
            //int count = 0;
            foreach (var fileLocation in _fileLocations)
            {
                //count++;
                //Console.WriteLine($"Reading File {count}");
                foreach (var PageTokens in GetTokens(ReadFile(fileLocation)))
                {
                    foreach (Match match in PageTokens)
                    {
                        var lower = match.Value.ToLower();

                        if (!Tokens.ContainsKey(lower))
                        {
                            Tokens.Add(lower, new MarkovDatum(lower));
                        }
                        if (CurrentTokenName != string.Empty)
                        {
                            Tokens[CurrentTokenName].RecordTrailing(Tokens[lower]);
                        }
                        CurrentTokenName = lower;
                    }
                }
            }
        }

        public IEnumerable<string> ReadPDFFile(string fileLocation)
        {
            PdfReader pdfReader = new PdfReader(fileLocation);

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                yield return PdfTextExtractor.GetTextFromPage(pdfReader, page);
            }
        }

        public IEnumerable<string> ReadTextFile(string fileLocation)
        {
            using (StreamReader sr = new StreamReader(fileLocation))
            {
                while(!sr.EndOfStream)
                {
                    yield return sr.ReadLine();
                }
            }
        }

        public IEnumerable<string> ReadFile(string fileLocation)
        {
            var fileExtension = System.IO.Path.GetExtension(fileLocation);
            if (fileExtension == ".pdf")
                return ReadPDFFile(fileLocation);

            if (fileExtension == ".txt")
                return ReadTextFile(fileLocation);

            throw new ArgumentException("Not a valid extension - select a .pdf or .txt document.", fileLocation);

        }

        public IEnumerable<MatchCollection> GetTokens(IEnumerable<string> pageStrings)
        {

            foreach (var Page in pageStrings)
            {
                yield return Regex.Matches(Page, "[\\w’]+|[,-.?!'\":;]");
                var x = Regex.Matches(Page, @"[\w’]+|[^\s\w]");
                //yield return x;
                //yield return (Page.Split()); //TODO: may want to rework this to also handle fullstops as a significant token in and of themselves.easiest way is probably to switch to 
                //regex. match ., insert ' ' before. Also want to do this for ! and ?. Basically all sentence terminators
            }
        }

        //public IEnumerable<string> GetSentences(int sentenceCount)
        //{
        //    //start with (but don't include) .
        //    MarkovStringGenerator generator = new MarkovStringGenerator();

        //    for (int i = 0; i < sentenceCount; i++)
        //    {
        //        yield return generator.BuildString(Tokens);
        //    }
        //}

        //private string _fileLocation;
        private IEnumerable<string> _fileLocations;
    }
}
