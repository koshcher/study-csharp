using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

namespace KeyboardTrainer
{
    class Generator
    {
        List<List<string>> words = new List<List<string>> { };
        Random rnd = new Random();
        public int Level { get; set; } // show what list we use
        public bool isLower { get; set; }

        public string Word { 
            get
            {
                if(isLower)
                {
                    return words[Level][rnd.Next(words[Level].Count)];
                }
                return words[Level][rnd.Next(words[Level].Count)].ToUpper();
            } 
        }

        public Generator()
        {
            isLower = true;
            // if we have internet connection will load files from url
            try
            {
                words.Add(ReadFromUrl("https://raw.githubusercontent.com/koshcher/english-word-lists/main/wordsA.txt"));
                words.Add(ReadFromUrl("https://raw.githubusercontent.com/koshcher/english-word-lists/main/wordsB.txt"));
                words.Add(ReadFromUrl("https://raw.githubusercontent.com/koshcher/english-word-lists/main/wordsC.txt"));
            }
            catch // if there are not internet will load from local files
            {
                words.Add(ReadFromFile("wordsA.txt"));
                words.Add(ReadFromFile("wordsB.txt"));
                words.Add(ReadFromFile("wordsC.txt"));
            }
        }

        List<string> ReadFromUrl(string url)
        {
            HttpClient client = new HttpClient();
            return (new List<string>(client.GetStringAsync(url).Result.Split('\n')));
        }
        List<string> ReadFromFile(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                return (new List<string>(sr.ReadToEnd().Split('\n')));
            }
        }

        /*
        public void New(int count)
        {
            generatedWords.Clear();
            Random rnd = new Random();
            for(int i = 0; i < count; i++)
            {
                generatedWords.Add(allWords[rnd.Next(allWords.Count)]);
            }
        }

        public override string ToString()
        {
            string res = "";
            foreach(var word in generatedWords)
            {
                res += word + " ";
            }
            return res;
        }*/
    }
}
