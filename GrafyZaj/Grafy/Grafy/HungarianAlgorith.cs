using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

            SubtractValuesInMatrix(v1,v2,graphMatrix);

            //0 - unmarked, 1 - starred, 2 - primed
            int[,] markMatrix = Utility.GraphTo2D_Matrix(v1, v2);
            int[,] coverMatrix = Utility.GraphTo2D_Matrix(v1, v2);

            int lineCount = CoverMatrix(v1, v2, graphMatrix, markMatrix, coverMatrix);

            while (lineCount < MathF.Min(v1.Count, v2.Count)) 
            {
                CoverWithNewValues(v1, v2, graphMatrix, coverMatrix);
                Utility.Show2D_Matrix(graphMatrix, v1, v2);
                lineCount = CoverMatrix(v1, v2, graphMatrix, markMatrix, coverMatrix);
            }

            //wynik koncowy
            int result = 0;
            Graph finalGraph = copyGraph.CopyGraphNodes();

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (graphMatrix[i, j] == 0)
                    {
                        finalGraph.AddNeighbor(i+1, (j + v1.Count + 1), graphMatrixCopy[i,j]);
                    }
                }
            }

            Dictionary<int, int> matches = MaximumAssociation.MaxAssociationAlgorithm(finalGraph);

            foreach (KeyValuePair<int, int> pair in matches)
            {
                result += graphMatrixCopy[pair.Key-1, (pair.Value-v1.Count-1)];
            }
            Console.WriteLine("Minimalny koszt: " + result);
        }

        private static void SubtractValuesInMatrix(List<Node> v1, List<Node> v2, int[,] graphMatrix)
        {
            int[,] minVertical = new int[v1.Count, 1];
            int[,] minHorizontal = new int[1, v2.Count];

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    minVertical[i, 0] = int.MaxValue;
                    minHorizontal[0, j] = int.MaxValue;
                }
            }

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (minVertical[i, 0] > graphMatrix[i, j] && graphMatrix[i, j] != int.MaxValue)
                    {
                        minVertical[i, 0] = graphMatrix[i, j];
                    }
                }
            }

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (minVertical[i, 0] != int.MaxValue && graphMatrix[i, j] != int.MaxValue)
                        graphMatrix[i, j] -= minVertical[i, 0];
                }
            }

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (minHorizontal[0, j] > graphMatrix[i, j] && graphMatrix[i, j] != int.MaxValue)
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
                        graphMatrix[i, j] -= minHorizontal[0, j];
                }
            }
            Utility.Show2D_Matrix(graphMatrix, v1, v2);
        }

        private static void CoverWithNewValues(List<Node> v1, List<Node> v2, int[,] graphMatrix, int[,] coverMatrix)
        {
            int minNonZero = int.MaxValue;
            //pokrywanie
            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (coverMatrix[i, j] == 0)
                    {
                        if (minNonZero > graphMatrix[i, j] && graphMatrix[i, j] != 0)
                        {
                            minNonZero = graphMatrix[i, j];
                        }
                    }
                }
            }
            Console.WriteLine(minNonZero);

            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (coverMatrix[i, j] == 1 && graphMatrix[i, j] != 0)
                    {
                        graphMatrix[i, j] += minNonZero;
                    }
                    else if (coverMatrix[i, j] == 0)
                    {
                        graphMatrix[i, j] -= minNonZero;
                    }
                }
            }
        }

        private static int CoverMatrix(List<Node> v1, List<Node> v2, int[,] graphMatrix, int[,] markMatrix, int[,] coverMatrix)
        {
            int[,] coverMatrixCounter = Utility.GraphTo2D_Matrix(v1, v2);
            //0 - unmarked, 1 - starred, 2 - primed
            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    markMatrix[i, j] = 0;
                    coverMatrix[i, j] = 0;
                    coverMatrixCounter[i,j] = 0;
                }
            }

            int countZerosInRow;
            for (int i = 0; i < v1.Count; i++)
            {
                countZerosInRow = 1;
                for (int j = 0; j < v2.Count; j++)
                {
                    if (graphMatrix[i, j] == 0)
                    {
                        markMatrix[i, j] = countZerosInRow;
                        countZerosInRow++;
                    }
                }

                for (int j = 0; j < v2.Count; j++)
                {
                    if (markMatrix[i, j] > 1 || countZerosInRow == 2)
                    {
                        markMatrix[i, j] = 0;
                    }
                }
            }

            //cover columns
            CoverColumns(coverMatrix, v1.Count, v2.Count, markMatrix, coverMatrixCounter);

            bool isCovered = CheckIfAllZerosAreCovered(coverMatrix, v1.Count, v2.Count, graphMatrix);
            int rowWith0 = -1;
            int colWith0 = -1;

            while (!isCovered)
            {
                PrimeUncoveredZero(coverMatrix, v1.Count, v2.Count, markMatrix, graphMatrix);

                for (int i = 0; i < v1.Count; i++)
                {
                    for (int j = 0; j < v2.Count; j++)
                    {
                        if (markMatrix[i, j] == 1)
                        {
                            rowWith0 = i; colWith0 = j;
                        }

                        if ((rowWith0 != -1 && colWith0 != -1) && (markMatrix[i, j] == 2 && graphMatrix[i, j] == 0))
                        {
                            //If the zero is on the same row as a starred zero
                            if (rowWith0 == i)
                            {
                                //uncover column
                                for (int k = 0; k < v1.Count; k++)
                                {
                                    coverMatrixCounter[k, colWith0]--;
                                    if (coverMatrixCounter[k, colWith0] == 0) coverMatrix[k, colWith0] = 0;
                                }
                                //cover row
                                for (int k = 0; k < v2.Count; k++)
                                {
                                    coverMatrixCounter[rowWith0, k]++;
                                    coverMatrix[rowWith0, k] = 1;
                                }
                                //Console.WriteLine("For row");
                                //Utility.Show2D_Matrix(coverMatrix, v1, v2);
                            }
                            else if (colWith0 == j)
                            {
                                //uncover row
                                for (int k = 0; k < v2.Count; k++)
                                {
                                    coverMatrixCounter[rowWith0, k]--;
                                    if (coverMatrixCounter[rowWith0, k] == 0) coverMatrix[rowWith0, k] = 0;
                                }
                                //cover column
                                for (int k = 0; k < v1.Count; k++)
                                {
                                    coverMatrixCounter[k, colWith0]++;
                                    coverMatrix[k, colWith0] = 1;
                                }
                                //Console.WriteLine("For col");
                                //Utility.Show2D_Matrix(coverMatrix, v1, v2);
                            }
                        }
                    }
                }
                rowWith0 = -1; colWith0 = -1;

                isCovered = CheckIfAllZerosAreCovered(coverMatrix, v1.Count, v2.Count, graphMatrix);

                //For all zeros encountered during the path, star primed zeros and unstar starred zeros
                for (int i = 0; i < v1.Count; i++)
                {
                    for (int j = 0; j < v2.Count; j++)
                    {
                        if (markMatrix[i, j] == 1) { markMatrix[i, j] = 0; }
                        else if (markMatrix[i, j] == 2) { markMatrix[i, j] = 1; }
                    }
                }

                CoverColumns(coverMatrix, v1.Count, v2.Count, markMatrix, coverMatrixCounter);
            }
            Console.WriteLine("CoverMatrix: ");
            Utility.Show2D_Matrix(coverMatrix, v1, v2);

            int lineCount = 0;
            int cntElements = 0;
            for (int i = 0; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    if (coverMatrix[i, j] == 1) 
                    {
                        cntElements++;
                    }
                }
                if (cntElements == v2.Count) lineCount++;
                cntElements = 0;
            }

            cntElements = 0;
            for (int j = 0; j < v2.Count; j++)
            {
                for (int i = 0; i < v1.Count; i++)
                {
                    if (coverMatrix[i, j] == 1)
                    {
                        cntElements++;
                    }
                }
                if (cntElements == v1.Count) lineCount++;
                cntElements = 0;
            }

            Console.WriteLine("Lines covered: " + lineCount);
            return lineCount;
        }

        private static void CoverColumns(int[,] coverMatrix, int v1count, int v2count, int[,] markMatrix, int[,] coverMatrixCounter)
        {
            for (int j = 0; j < v1count; j++)
            {
                for (int i = 0; i < v2count; i++)
                {
                    if (markMatrix[i, j] == 1 && coverMatrix[i,j] == 0)
                    {
                        for (int k = 0; k < v1count; k++)
                        {
                            coverMatrix[k, j] = 1;
                            coverMatrixCounter[k, j]++;
                        }
                        break;
                    }
                }
            }
        }

        private static void PrimeUncoveredZero(int[,] coverMatrix, int v1count, int v2count, int[,] markMatrix, int[,] graphMatrix)
        {
            for (int i = 0; i < v1count; i++)
            {
                for (int j = 0; j < v2count; j++)
                {
                    if (markMatrix[i, j] == 0 && coverMatrix[i, j] == 0 && graphMatrix[i,j] == 0)
                    {
                        markMatrix[i, j] = 2;
                        return;
                    }
                }
            }
        }

        private static bool CheckIfAllZerosAreCovered(int[,] coverMatrix, int v1count, int v2count, int[,] graphMatrix)
        {
            bool allCovered = true;
            for (int i = 0; i < v1count; i++)
            {
                for (int j = 0; j < v2count; j++)
                {
                    if (graphMatrix[i,j] == 0 && coverMatrix[i,j] != 1)
                    {
                        allCovered = false;
                        break;
                    }
                }
            }

            return allCovered;
        }
    }
}
