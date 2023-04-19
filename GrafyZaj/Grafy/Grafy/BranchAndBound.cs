using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    internal class BranchAndBound
    {
        private static int[,] GraphTo2D_Matrix(Graph graph)
        {
            int[,] matrix = new int[graph.GetNodeCount(), graph.GetNodeCount()];
            List<Node> v1 = graph.GetNodeList();

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                for (int j = 0; j < graph.GetNodeCount(); j++)
                {
                    matrix[i, j] = int.MaxValue;
                }
            }

            int it = 0;
            foreach (Node node in v1)
            {
                foreach (int neighbor in node.Neighbors)
                {
                    matrix[(neighbor - 1), it] = node.EdgeValues[neighbor];
                }
                it++;
            }

            return matrix;
        }
        private static int HungarianAlgShort(Graph graph, int[,] graphMatrix)
        {
            int[,] minVertical = new int[graph.GetNodeCount(), 1];
            int[,] minHorizontal = new int[1, graph.GetNodeCount()];

            int LB = 0;

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                for (int j = 0; j < graph.GetNodeCount(); j++)
                {
                    minVertical[i, 0] = int.MaxValue;
                    minHorizontal[0, j] = int.MaxValue;
                }
            }

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                for (int j = 0; j < graph.GetNodeCount(); j++)
                {
                    if (minVertical[i, 0] > graphMatrix[i, j] && graphMatrix[i, j] != int.MaxValue)
                    {
                        minVertical[i, 0] = graphMatrix[i, j];
                    }
                }
            }

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                for (int j = 0; j < graph.GetNodeCount(); j++)
                {
                    if (minVertical[i, 0] != int.MaxValue && graphMatrix[i, j] != int.MaxValue)
                        graphMatrix[i, j] -= minVertical[i, 0];
                }
            }

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                for (int j = 0; j < graph.GetNodeCount(); j++)
                {
                    if (minHorizontal[0, j] > graphMatrix[i, j] && graphMatrix[i, j] != int.MaxValue)
                    {
                        minHorizontal[0, j] = graphMatrix[i, j];
                    }
                }
            }

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                for (int j = 0; j < graph.GetNodeCount(); j++)
                {
                    if (minHorizontal[0, j] != int.MaxValue && graphMatrix[i, j] != int.MaxValue)
                        graphMatrix[i, j] -= minHorizontal[0, j];
                }
            }

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                LB += minVertical[i, 0];
            }

            for (int j = 0; j < graph.GetNodeCount(); j++)
            {
                LB += minHorizontal[0, j];
            }

            return LB;
        }
        /*private static int CalculateCost(Graph graph, int[,] graphMatrix, int[,] graphMatrixCopy)
        {
            int result = 0;

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                for (int j = 0; j < graph.GetNodeCount(); j++)
                {
                    if (graphMatrix[i, j] == 0)
                    {
                        result += graphMatrixCopy[i, j];
                    }
                }
            }
            Console.WriteLine("Minimalny koszt: " + result);

            return result;
        }*/
        private static int CalculateNewValues(Graph graph, int[,] graphMatrix, int x, int y)
        {
            int minx = int.MaxValue, miny = int.MaxValue;
            int LB = 0;

            for (int k = 0; k < graph.GetNodeCount(); k++)
            {
                if (minx > graphMatrix[x, k] && graphMatrix[x, k] >= 0) minx = graphMatrix[x, k];
                if (miny > graphMatrix[k, y] && graphMatrix[k, y] >= 0) miny = graphMatrix[k, y];
            }

            for (int k = 0; k < graph.GetNodeCount(); k++)
            {
                if(graphMatrix[x, k] != int.MaxValue && graphMatrix[x, k] > 0) graphMatrix[x, k] -= minx;
                if(graphMatrix[k, y] != int.MaxValue && graphMatrix[k, y] > 0) graphMatrix[k, y] -= miny;
            }

            LB = minx + miny;

            //sometimes gives a - answear, fix!-
            return LB;
        }
        private static void EraseRowAndCol(Graph graph, int[,] graphMatrix, Dictionary<int,int> pairs)
        {
            int minx = int.MaxValue, miny = int.MaxValue;
            int max = 0;
            int iMax = 0, jMax = 0;
            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                for (int j = 0; j < graph.GetNodeCount(); j++)
                {
                    if (graphMatrix[i, j] == 0)
                    {
                        minx = int.MaxValue;  miny = int.MaxValue;
                        for (int k = 0; k < graph.GetNodeCount(); k++)
                        {
                            if(minx > graphMatrix[i, k] && graphMatrix[i, k] > 0 && graphMatrix[i, k] != int.MaxValue) minx = graphMatrix[i, k];
                            if(miny > graphMatrix[k, j] && graphMatrix[k, j] > 0 && graphMatrix[k, j] != int.MaxValue) miny = graphMatrix[k, j];
                        }

                        if (max < miny) { max = miny; iMax = i; jMax = j; }
                        else if (max < minx) { max = minx; iMax = i; jMax = j; }
                    }
                }
            }
            pairs.Add(iMax, jMax);

            if (max != int.MaxValue)
            {
                Console.WriteLine("Max value: " + max + " on: (" + iMax + "," + jMax + ")");
            }
            else
            {
                Console.WriteLine("Only zeros left, removing on: (" + iMax + "," + jMax + ")");
            }

            //replace col and row with -1
            for (int k = 0; k < graph.GetNodeCount(); k++)
            {
                graphMatrix[iMax, k] = int.MaxValue;
                graphMatrix[k, jMax] = int.MaxValue;
            }
        }
        public static void BranchAndBoundVersion(Graph graph)
        {
            int result = 0, resultTmp = 0;
            Dictionary<int, int> path = new Dictionary<int, int>();
            int lastResult = 0, finalResult = 0; ;

            int[,] graphMatrix = GraphTo2D_Matrix(graph);
            int[,] graphMatrixCopy = GraphTo2D_Matrix(graph);
            int[,] tmpMatrix = GraphTo2D_Matrix(graph);

            Console.WriteLine("Start Matrix: ");
            Utility.Show2D_Matrix_Graph(graphMatrix, graph);

            //step 0
            result = HungarianAlgShort(graph, graphMatrix);
            Utility.Show2D_Matrix_Graph(graphMatrix, graph);
            //result = CalculateCost(graph, graphMatrix, graphMatrixCopy);
            lastResult = result;

            int matrixSize = graph.GetNodeCount();
            while(matrixSize > 1)
            {
                CopyArray2D(graphMatrix, tmpMatrix, graph.GetNodeCount());

                EraseRowAndCol(graph, graphMatrix, path);
                result = lastResult + HungarianAlgShort(graph, graphMatrix);
                Utility.Show2D_Matrix_Graph(graphMatrix, graph);

                Console.WriteLine("res: " + lastResult + " " + result);

                tmpMatrix[path.Last().Key, path.Last().Value] = int.MaxValue;
                Utility.Show2D_Matrix_Graph(tmpMatrix, graph);
                //resultTmp = CalculateCost(graph, tmpMatrix, graphMatrixCopy);
                resultTmp = lastResult + CalculateNewValues(graph, tmpMatrix, path.Last().Key, path.Last().Value);
                Console.WriteLine("restmp: " + lastResult + " " + resultTmp);

                if (resultTmp < result && matrixSize > 2)
                {
                    result = resultTmp;
                    path.Remove(path.Last().Key);
                    CopyArray2D(tmpMatrix, graphMatrix, graph.GetNodeCount());
                }
                else matrixSize--;

                lastResult = result;
            }

            Console.WriteLine("Final result: " + lastResult);

            Console.WriteLine("Pairs: ");
            foreach(int key in path.Keys)
            {
                Console.Write(" (" + key + " , " + path[key] + ") ");
            }
        }

        private static void CopyArray2D(int[,] copied, int[,] copiedTo, int x)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    copiedTo[i,j] = copied[i,j];
                }
            }
        }
    }
}
