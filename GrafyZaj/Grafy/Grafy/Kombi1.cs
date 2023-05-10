using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    public class Kombi1
    {
        private static int SubtractValuesInMatrix(int matrixSize, int[,] graphMatrix, Graph graph)
        {
            int[,] minVertical = new int[matrixSize, 1];
            int[,] minHorizontal = new int[1, matrixSize];
            int LB = 0;

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    minVertical[i, 0] = int.MaxValue;
                    minHorizontal[0, j] = int.MaxValue;
                }
            }

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (minVertical[i, 0] > graphMatrix[i, j] && graphMatrix[i, j] != int.MaxValue)
                    {
                        minVertical[i, 0] = graphMatrix[i, j];
                    }
                }
            }

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (minVertical[i, 0] != int.MaxValue && graphMatrix[i, j] != int.MaxValue)
                    {
                        graphMatrix[i, j] -= minVertical[i, 0];
                    }
                }
            }

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (minHorizontal[0, j] > graphMatrix[i, j] && graphMatrix[i, j] != int.MaxValue)
                    {
                        minHorizontal[0, j] = graphMatrix[i, j];
                    }
                }
            }

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (minHorizontal[0, j] != int.MaxValue && graphMatrix[i, j] != int.MaxValue)
                    {
                        graphMatrix[i, j] -= minHorizontal[0, j];
                    }
                }
            }
            //Utility.Show2D_Matrix_Graph(graphMatrix, graph);

            for (int i = 0; i < matrixSize; i++)
            {
                if (minVertical[i, 0] == int.MaxValue)
                    minVertical[i, 0] = 0;
                if (minHorizontal[0, i] == int.MaxValue)
                    minHorizontal[0, i] = 0;
            }

            for (int i = 0; i < matrixSize; i++)
            {
                LB += minVertical[i, 0];
                LB += minHorizontal[0, i];
            }

            return LB;
        }
        private static List<int> CoverMatrix(int matrixSize, int[,] graphMatrix, Dictionary<int, int> path)
        {
            int minRow = int.MaxValue;
            int minColumn = int.MaxValue;
            int savedi = -1, savedj = -1;

            int max = 0;

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (graphMatrix[i,j] == 0 &&
                        path.ContainsKey(i) == false &&
                        ((path.ContainsKey(i) && path[i] == j)
                                || (path.ContainsKey(j) && path[j] == i)) == false)
                        {
                        minRow = int.MaxValue;
                        minColumn = int.MaxValue;
                        for (int k = 0; k < matrixSize; k++)
                        {
                            if (graphMatrix[k, j] < minRow && graphMatrix[k, j] != 0)
                            {
                                minRow = graphMatrix[k, j];
                            }
                        }

                        for (int k = 0; k < matrixSize; k++)
                        {
                            if (graphMatrix[i, k] < minColumn && graphMatrix[i, k] != 0)
                            {
                                minColumn = graphMatrix[i, k];
                            }
                        }

                        if(minColumn > max)
                        {
                            if (((path.ContainsKey(i) && path[i] == j) 
                                || (path.ContainsKey(j) && path[j] == i)) == false)
                            { 
                                max = minColumn;
                                savedi = i;
                                savedj = j;
                            }
                        }
                        if(minRow > max)
                        {
                            if (((path.ContainsKey(i) && path[i] == j)
                                || (path.ContainsKey(j) && path[j] == i)) == false)
                            {
                                max = minRow;
                                savedi = i;
                                savedj = j;
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < matrixSize; k++)
            {
                graphMatrix[k, savedj] = int.MaxValue;
                graphMatrix[savedi, k] = int.MaxValue;
            } 

            path.Add(savedi, savedj);

            List<int> choosenPoint = new List<int>();
            choosenPoint.Add(savedi);
            choosenPoint.Add(savedj);

            return choosenPoint;
        }
        public static int[,] GraphTo2D_Matrix(List<Node> v1, List<Node> v2)
        {
            int[,] matrix = new int[v1.Count, v2.Count];

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if(i == j) matrix[i, j] = int.MaxValue;
                    else matrix[i, j] = 0;
                }
            }

            int it = 0;
            foreach (Node node in v1)
            {
                foreach (int neighbor in node.Neighbors)
                {
                    matrix[node.NodeNumber - 1, neighbor - 1] = node.EdgeValues[neighbor];
                }
                it++;
            }

            return matrix;
        }
        public static int GetMatrixSize(int[,] graphMatrix)
        {
            int coveredCount = 0;
            for (int k = 0; k < graphMatrix.GetLength(0); k++)
            {
                if (graphMatrix[k, 0] == int.MaxValue)
                {
                    int cnt = 0;
                    for (int k2 = 0; k2 < graphMatrix.GetLength(0); k2++)
                    {
                        if (graphMatrix[k, k2] == int.MaxValue) cnt++;
                        else break;
                    }
                    if (cnt == graphMatrix.GetLength(0)) coveredCount++;
                }
            }

            int matrixSize = graphMatrix.GetLength(0) - coveredCount;
            return matrixSize;
        }
        public static void Kombi(Graph graph)
        {
            BinaryTree tree = new BinaryTree();
            Dictionary<int[,], int> dictOfTables = new Dictionary<int[,], int>();
            Dictionary<int,int> path = new Dictionary<int,int>();
            List<Dictionary<int, int>> paths = new List<Dictionary<int, int>>();

            Graph copyGraph = graph.CopyGraph();
            int[,] graphMatrix = GraphTo2D_Matrix(copyGraph.GetNodeList(), copyGraph.GetNodeList());
            Utility.Show2D_Matrix_Graph(graphMatrix, copyGraph);

            int matrixSize = copyGraph.GetNodeCount();
            int LB = SubtractValuesInMatrix(copyGraph.GetNodeCount(), graphMatrix, graph);
            List<int> coveredPoint = CoverMatrix(copyGraph.GetNodeCount(), graphMatrix, path);
            //Utility.Show2D_Matrix_Graph(graphMatrix, copyGraph);
            matrixSize--;

            tree.Insert(LB);
            paths.Add(path);
            dictOfTables.Add(graphMatrix, LB);

            while (matrixSize > 0) 
            {
                int[,] graphMatrixIfAdded = Utility.CopyMatrix(graphMatrix, copyGraph.GetNodeCount());
                int[,] graphMatrixIfRemoved = Utility.CopyMatrix(graphMatrix, copyGraph.GetNodeCount());
                
                //remove row and col
                int LBifAdded = LB + SubtractValuesInMatrix(copyGraph.GetNodeCount(), graphMatrixIfAdded, copyGraph);
                coveredPoint = CoverMatrix(copyGraph.GetNodeCount(), graphMatrixIfAdded, path);
                //Utility.Show2D_Matrix_Graph(graphMatrixIfAdded, copyGraph);

                tree.Insert(LBifAdded);
                paths.Add(path);
                dictOfTables.Add(graphMatrixIfAdded, LBifAdded);
                
                //remove only 1 value
                graphMatrixIfRemoved[coveredPoint[0], coveredPoint[1]] = int.MaxValue;
                int LBifRemoved = LB + SubtractValuesInMatrix(copyGraph.GetNodeCount(), graphMatrixIfRemoved, copyGraph);
                //Utility.Show2D_Matrix_Graph(graphMatrixIfRemoved, copyGraph);
                
                tree.Insert(LBifRemoved);
                Dictionary<int, int> pathTmp = new Dictionary<int, int>(path);
                pathTmp.Remove(0);
                paths.Add(pathTmp);
                dictOfTables.Add(graphMatrixIfRemoved, LBifRemoved);

                //check which is smaller
                if(LBifRemoved >= LBifAdded)
                {
                    LB = LBifAdded;
                    graphMatrix = graphMatrixIfAdded;
                    matrixSize--;
                }
                else
                {
                    LB = LBifRemoved;
                    graphMatrix = graphMatrixIfRemoved;
                }

                TreeNode minLeaf = tree.GetLeaves(tree._root).Min();
                if (minLeaf != null)
                {
                    if (minLeaf.Data < LB)
                    {
                        //Console.WriteLine(tree.MinValue(tree._root).Index);
                        graphMatrix = dictOfTables.Keys.ElementAt(minLeaf.Index);
                        matrixSize = GetMatrixSize(graphMatrix);
                        path = paths[minLeaf.Index];
                    }
                }
            }

            Graph resultGraph = copyGraph.CopyGraphNodes();
            int[,] graphMatrixCopy = GraphTo2D_Matrix(copyGraph.GetNodeList(), copyGraph.GetNodeList());
            int finalCost = 0;
            foreach(KeyValuePair<int,int> nodeIndexes in path)
            {
                resultGraph.AddNeighbor(resultGraph.GetNodeList()[nodeIndexes.Key].NodeNumber, resultGraph.GetNodeList()[nodeIndexes.Value].NodeNumber, 0);
                finalCost += graphMatrixCopy[nodeIndexes.Key, nodeIndexes.Value];
            }
            Console.WriteLine("Graf koncowy: ");
            resultGraph.ShowGraphByNodes();

            if (Euler.EulerMethod(resultGraph))
                Console.WriteLine("Koszt sciezki: " + finalCost);
            else
                Console.WriteLine("Brak pelnej sciezki!");
        }
    }
}
