using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class DecisionTree
    {
        public int numNodes;
        public int numClasses;
        public List<Node> tree;

        public DecisionTree(int numNodes, int numClasses)
        {
            this.numNodes = numNodes;
            this.numClasses = numClasses;
            this.tree = new List<Node>();
            for (int i = 0; i < numNodes; ++i)
                this.tree.Add(new Node());
        }

        public void BuildTree(double[][] dataX, int[] dataY)
        {
         
            int n = dataX.Length;

            List<int> allRows = new List<int>();
            for (int i = 0; i < n; ++i)
                allRows.Add(i);

            this.tree[0].rows = new List<int>(allRows);

            for (int i = 0; i < this.numNodes; ++i) 
            {
                this.tree[i].nodeID = i;

                SplitInfo si = GetSplitInfo(dataX, dataY, this.tree[i].rows, this.numClasses);  
                tree[i].splitCol = si.splitCol;
                tree[i].splitVal = si.splitVal;

                tree[i].classCounts = ComputeClassCts(dataY, this.tree[i].rows, this.numClasses);
                tree[i].predictedClass = ArgMax(tree[i].classCounts);

                int leftChild = (2 * i) + 1;
                int rightChild = (2 * i) + 2;

                if (leftChild < numNodes)
                    tree[leftChild].rows = new List<int>(si.lessRows);
                if (rightChild < numNodes)
                    tree[rightChild].rows = new List<int>(si.greaterRows);
            } 

        } 

        public int Predict(double[] x, bool verbose)
        {
            bool vb = verbose;
            int result = -1;
            int currNodeID = 0;
           
            while (true)
            {
                if (this.tree[currNodeID].rows.Count == 0)  
                    break;

               

                int sc = this.tree[currNodeID].splitCol;
                double sv = this.tree[currNodeID].splitVal;
                double v = x[sc];
                

                if (v < sv)
                {
                    

                    currNodeID = (2 * currNodeID) + 1;
                    if (currNodeID >= this.tree.Count)
                        break;
                    result = this.tree[currNodeID].predictedClass;
                   
                }
                else
                {
                    
                    currNodeID = (2 * currNodeID) + 2;
                    if (currNodeID >= this.tree.Count)
                        break;
                    result = this.tree[currNodeID].predictedClass;
                  
                }

                
            }
            return result;
        } 


        private static SplitInfo GetSplitInfo(double[][] dataX, int[] dataY, List<int> rows, int numClasses)
        {
            
            int nCols = dataX[0].Length;
            SplitInfo result = new SplitInfo();

            int bestSplitCol = 0;
            double bestSplitVal = 0.0;
            double bestImpurity = double.MaxValue;
            List<int> bestLessRows = new List<int>();
            List<int> bestGreaterRows = new List<int>(); 

            foreach (int i in rows)  
            {
                for (int j = 0; j < nCols; ++j)
                {
                    double splitVal = dataX[i][j];  
                    List<int> lessRows = new List<int>();
                    List<int> greaterRows = new List<int>();
                    foreach (int ii in rows)  
                    {
                        if (dataX[ii][j] < splitVal)
                            lessRows.Add(ii);
                        else
                            greaterRows.Add(ii);
                    } 

                    double meanImp = MeanImpurity(dataY, lessRows, greaterRows, numClasses);

                    if (meanImp < bestImpurity)
                    {
                        bestImpurity = meanImp;
                        bestSplitCol = j;
                        bestSplitVal = splitVal;

                        bestLessRows = new List<int>(lessRows);  
                        bestGreaterRows = new List<int>(greaterRows);
                    }

                } 
            } // i

            result.splitCol = bestSplitCol;
            result.splitVal = bestSplitVal;
            result.lessRows = new List<int>(bestLessRows);
            result.greaterRows = new List<int>(bestGreaterRows);

            return result;
        }

        private static double Impurity(int[] dataY, List<int> rows, int numClasses)
        {
            

            if (rows.Count == 0) return 0.0;

            int[] counts = new int[numClasses];  
            double[] probs = new double[numClasses];  
            for (int i = 0; i < rows.Count; ++i)
            {
                int idx = rows[i]; 
                int c = dataY[idx] + 1;  
                ++counts[c];
            }

            for (int c = 0; c < numClasses; ++c)
                if (counts[c] == 0) probs[c] = 0.0;
                else probs[c] = (counts[c] * 1.0) / rows.Count;

            double sum = 0.0;
            for (int c = 0; c < numClasses; ++c)
                sum += probs[c] * probs[c];

            return 1.0 - sum;
        }

        private static double MeanImpurity(int[] dataY, List<int> rows1, List<int> rows2, int numClasses)
        {
            if (rows1.Count == 0 && rows2.Count == 0)
                return 0.0;

            double imp1 = Impurity(dataY, rows1, numClasses); 
            double imp2 = Impurity(dataY, rows2, numClasses);
            int count1 = rows1.Count;
            int count2 = rows2.Count;
            double wt1 = (count1 * 1.0) / (count1 + count2);
            double wt2 = (count2 * 1.0) / (count1 + count2);
            double result = (wt1 * imp1) + (wt2 * imp2);
            return result;
        }

        private static int[] ComputeClassCts(int[] dataY, List<int> rows, int numClasses)
        {
            int[] result = new int[numClasses];
            foreach (int i in rows)
            {
                int c = dataY[i] + 1;
                ++result[c];
            }
            return result;
        }

        private static int ArgMax(int[] classCts)
        {
            int largeCt = 0;
            int largeIndx = 0;
            for (int i = 0; i < classCts.Length; ++i)
            {
                if (classCts[i] > largeCt)
                {
                    largeCt = classCts[i];
                    largeIndx = i;
                }
            }
            return largeIndx;
        }


        public class Node
        {
            public int nodeID;
            public List<int> rows; 
            public int splitCol;
            public double splitVal;
            public int[] classCounts;
            public int predictedClass;
        }

        public class SplitInfo  
        {
            public int splitCol;
            public double splitVal;
            public List<int> lessRows;
            public List<int> greaterRows;
        }
    }
}
