using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class RF
    {
        List<DecisionTree> dtl = new List<DecisionTree>();
  
        public static void Sample(double[][] dX, int[] y, int n, double[][] nm, int[] nl)
        {
            Random random = new Random();
            for (int i = 0; i < dX.Length / n; i++)
            {
                int temp = random.Next(0, dX.Length - 1);
                nm[i] = dX[temp];
                nl[i] = y[temp];
            }
        }
        public static void BuildForest(int n_trees, int depth, double[][] x, int[] y, double[][] x1, int[] y1, List<DecisionTree> dtl)
        {
            for (int i = 0; i < n_trees; i++)
            {
                Sample(x, y, 200, x1, y1);
                DecisionTree dt = new DecisionTree(depth, 3);
                dt.BuildTree(x1, y1);
                dtl.Add(dt);
            }
        }

        public static string Vote(double[] x, List<DecisionTree> dtl)
        {   string result;
            List<int> res = new List<int>();
            foreach (var d in dtl)
                res.Add(d.Predict(x, true));
            Dictionary<int, int> res2 = new Dictionary<int, int>();
            res2.Add(2, 0);
            res2.Add(1, 0);
            res2.Add(0, 0);
            for (int i = 0; i < res.Count; i++)
                res2[res[i]]++;
            if (res2[0] > res2[1] && res[0] > res[2])
                result = "негатив";
            if (res[2] > res[1] && res[2] > res[0])
                result = "позитив";
            else result = "unclear....";
            return result;

        }
    }
}
