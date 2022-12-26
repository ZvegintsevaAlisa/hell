using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    
    public class PrepareData
    {
       
       public void Prepare(string file, int[] label, List<KeyValuePair<string, int>> fr_words, List<List<string>> fulltext)
        {
            string[] stopword = { "отель", "останавливались", "из", "для", "по", "если", "выбрали", "это", "он", "что", "персонал", "номер", "на", "отдыхали", "приехали", "были", "женой", "мужем", "детьми", " " };
            List<string> sw = new List<string>();
            foreach (var w in stopword)
                sw.Add(Porter.TransformingWord(w));
            string[] input = { " ", " " };
            //List<string> uwords = new List<string>();

            int count = 0;


            var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);


   


          //  List<List<string>> fulltext = new List<List<string>>();
            List<string> all = new List<string>();
            Dictionary<string, int> word_fr = new Dictionary<string, int>();
            //int[] cl = new int[8486];
            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    count++;

                    input = sr.ReadLine().Split(' ');
                    int val = Convert.ToInt32(input.Last());
                    List<string> lines = new List<string>();

                    label[count - 1] = val;

                    fulltext.Add(lines);

                    foreach (var word in input)
                    {
                        if (word.Length >= 2)
                        {
                            string temp = Porter.TransformingWord(word);
                            if (!sw.Contains(temp))
                            {
                                if (!lines.Contains(temp)) if (word_fr.ContainsKey(temp)) word_fr[temp]++;
                                    else word_fr.Add(temp, 1);
                                lines.Add(temp);
                            }

                        }

                    }
                }

            }

            
            //fr_words = word_fr.ToList();
            foreach( KeyValuePair<string, int > word in word_fr)
                fr_words.Add(new KeyValuePair<string, int>(word.Key, word.Value));

            fr_words.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            fr_words.RemoveRange(0, fr_words.Count - 900);
        }

    }
}
