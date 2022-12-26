using ConsoleApp2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp2
{ class RandomForest
    {
        public static void CreateForest(List<KeyValuePair<string, int>> fr_words, int[] label, List<List<string>> fulltext)
        {   List<DecisionTree> dtl = new List<DecisionTree>();
            RF forest = new RF();
            double[][] mat = new double[label.Length][];
            TFIDF(fr_words, label, fulltext, mat);
            Console.WriteLine("Введите строку:");
            string input = Console.ReadLine();
            double[] vec = Vectorize(input, fr_words, fulltext, label);
            double[][]  smpl = new double[mat.Length / 200][];
            for (int i = 0; i < smpl.Length; i++)
                smpl[i] = new double[900];
            int[] nl = new int[mat.Length / 200];
            RF.BuildForest(70, 7, mat, label, smpl, nl, dtl);
           string res = RF.Vote(vec, dtl);
            Console.WriteLine(res);
        }
        static void TFIDF(List<KeyValuePair<string, int>> fr_words, int[] label, List<List<string>> fulltext, double[][] mat)
        {

            int count = label.Length;
            int q = 0;
            double tf = 0;
            double log = 0;
 

            for (int i = 0; i < count; i++)
            {
                mat[i] = new double[900];
                for (int j = 0; j < 900; j++)
                {
                    q = fulltext[i].Where(c => c.Equals(fr_words[j].Key)).Count();
                    tf = (double)q / (double)fulltext.Count();
                    log = Math.Log10((double)count / (double)fr_words[j].Value);
                    double res = tf * log;
                    mat[i][j] = res;
                }
            }



           
        }
        public static double[] Vectorize(string input, List<KeyValuePair<string, int>> fr_words, List<List<string>> fulltext, int[] lable)
        {
            string[] str = input.Split(' ');
            string temp = " ";
            int count = lable.Length;
            for (int i = 0; i < str.Length; i++)
            {
                temp = Porter.TransformingWord(str[i]);
                str[i] = temp;
            }
            int q = 0;
            double tf = 0;
            double log = 0;
            double[] vec = new double[900];
            for (int j = 0; j < 900; j++)
            {
                if (str.Where(c => c.Equals(fr_words[j].Key)).Count() == 0)
                    vec[j] = 0.0;
                else
                    q = str.Where(c => c.Equals(fr_words[j].Key)).Count();
                tf = (double)q / (double)fulltext.Count();
                log = Math.Log10((double)count / (double)fr_words[j].Value);
                double res = tf * log;
                vec[j] = res;
            }
            return vec;
        }
    }
}

