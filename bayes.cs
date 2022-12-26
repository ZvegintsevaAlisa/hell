using ConsoleApp2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Bayes
    {
        

        List<string> positive = new List<string>();
        List<string> negative = new List<string>();
        List<string> neutral = new List<string>();
        List<string> allwords = new List<string>();

        double negprob;
        double posprob;
        double neutprob;

        double allandpos;
        double allandneg;
        double allandneut;
       public void SelectFeatures(List<List<string>> fulltext, int[] label)
        {
            for (int i = 0; i < fulltext.Count; i++)
                for (int j = 0; j < fulltext[i].Count; j++)
                { if (!allwords.Contains(fulltext[i][j])) allwords.Add(fulltext[i][j]);
                    if (label[i] == 0) neutral.Add(fulltext[i][j]);
                    if (label[i] > 0) positive.Add(fulltext[i][j]);
                    if (label[i] < 0) negative.Add(fulltext[i][j]);
                }
        }

        public void CalculateProb(int[] label)
        {

            int pos_docs = 0; int neg_docs = 0; int neut_docs = 0; int all_words = allwords.Count;
            for (int i = 0; i < label.Length; i++)
            {
                if (label[i] == 0) neut_docs++;
                else if (label[i] < 0) neg_docs++;
                else if (label[i] > 0) pos_docs++;
            }
            int all_docs = pos_docs + neg_docs + neut_docs;

            negprob = (double)neg_docs / (double)all_docs;
            posprob = (double)pos_docs / (double)all_docs;
            neutprob = (double)neut_docs / (double)all_docs;

            allandpos = positive.Count() + allwords.Count();
            allandneg = negative.Count() + allwords.Count();
            allandneut = neutral.Count() + allwords.Count();
          
        }

        public void BayesMethod(string input)
        {

            List<double> pos_prob = new List<double>();
            List<double> neg_prob = new List<double>();
            List<double> neut_prob = new List<double>();

            string temp = " ";
            string[] str = input.Split(' ');
            for (int i = 0; i < str.Length; i++)
            {
                temp = Porter.TransformingWord(str[i]);
                str[i] = temp;
            }
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (allwords.Contains(str[i]))
                {
                    int q = IsIt(positive, str[i]);
                    double q1 = (double)(q + 1) / allandpos;
                    pos_prob.Add(q1);
                    // if (negwords.Contains(word))
                    int r = IsIt(negative, str[i]);
                    double r1 = (double)(r + 1) / allandneg;
                    neg_prob.Add(r1);
                    //if (neutwords.Contains(word))
                    int y = IsIt(neutral, str[i]);
                    double y1 = (y + 1) / allandneut;
                    neut_prob.Add(y1);
                }
                else count++;
              }
            if (count == str.Length)
                throw new nwException();
                    double m1 = 1; double m2 = 1; double m3 = 1;

                    foreach (double t in pos_prob)
                        m1 *= t;
                    foreach (double t in neg_prob)
                        m2 *= t;
                    foreach (double t in neut_prob)
                        m3 *= t;
                    double pprob = posprob * m1;
                    double nprob = negprob * m2;
                    double neprob = neutprob * m3;
            if (pprob > nprob && pprob > neprob)
                Console.WriteLine("Самый вероятный класс: позитив");
            else if (nprob > pprob && nprob > neprob)
                Console.WriteLine("Самый вероятный класс: негатив");
            else Console.WriteLine("Самый вероятный класс: нейтрал");
               }

        static int IsIt(List<string> a, string b)
        {
            int q = a.Where(c => c.Equals(b)).Count();
            return q;
        }
    }
            
  }
    
    


