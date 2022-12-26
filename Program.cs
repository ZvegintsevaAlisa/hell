using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string file = Path.Combine(path, "hdata.txt");
            int[] label = new int[8728];
            List < KeyValuePair<string, int>> fr_words= new List<KeyValuePair<string, int>>(); 
            List<List<string>> fulltext = new List<List<string>>();

            PrepareData prepareData = new PrepareData();
            prepareData.Prepare(file, label, fr_words, fulltext);


            Bayes bayes = new Bayes();
            bayes.SelectFeatures(fulltext, label);
            bayes.CalculateProb(label);

            Console.WriteLine("Введите отзыв:");
            string input = Console.ReadLine();

            //try
            //{
            //    bayes.BayesMethod(input);
            //}
            //catch (nwException e)
            //{ Console.WriteLine(e.Message); }

            RandomForest.CreateForest(fr_words, label, fulltext);

            Console.ReadLine();
        }
    }
    class nwException : Exception
    {
        public nwException() : base("Программа не знает таких слов...") { }
    }
}
