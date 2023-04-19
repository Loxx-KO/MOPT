using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    public class HungarianAlgorith
    {
        public static void HungarianAlgorithm(Graph graph) 
        {
            Graph copyGraph = graph.CopyGraph();

            if (Utility.CheckIfGraphIsBiparted(copyGraph, true))
            {
                copyGraph.ShowGraphByNodes();
            }
            else return;

            List<Node> copyGraphAlgList = copyGraph.GetNodeList();
            List<Node> v1 = new List<Node>();
            List<Node> v2 = new List<Node>();

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                if (copyGraphAlgList[i].Value == 1)
                {
                    v1.Add(copyGraphAlgList[i]);
                }
                else
                {
                    v2.Add(copyGraphAlgList[i]);
                }
            }

            int[,] graphMatrix =  Utility.GraphTo2D_Matrix(v1, v2);
            int[,] graphMatrixCopy = Utility.GraphTo2D_Matrix(v1, v2);
            Utility.Show2D_Matrix(graphMatrix, v1, v2);

            int[,] minVertical = new int[v1.Count,1];
            int[,] minHorizontal = new int[1,v2.Count];

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    minVertical[i, 0] = int.MaxValue;
                    minHorizontal[0 ,j] = int.MaxValue;
                }
            }

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if(minVertical[i,0] > graphMatrix[i,j] && graphMatrix[i, j] != int.MaxValue)
                    {
                        minVertical[i,0] = graphMatrix[i,j];
                    }
                }
            }

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if(minVertical[i, 0] != int.MaxValue && graphMatrix[i, j] != int.MaxValue)
                        graphMatrix[i, j] -= minVertical[i, 0];
                }
            }

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (minHorizontal[0, j] > graphMatrix[i, j] && graphMatrix[i,j] != int.MaxValue)
                    {
                        minHorizontal[0, j] = graphMatrix[i, j];
                    }
                }
            }

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (minHorizontal[0, j] != int.MaxValue && graphMatrix[i, j] != int.MaxValue)
                        graphMatrix[i,j] -= minHorizontal[0, j];
                }
            }

            Utility.Show2D_Matrix(graphMatrix, v1, v2);

            int result = 0;

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (graphMatrix[i, j] == 0)
                    {
                        result += graphMatrixCopy[i, j];
                    }
                }
            }

            Console.WriteLine("Minimalny koszt: " + result);
        }
    }
}
