using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Grafy
{
    internal class ColorGraph
    {
        public static void ColorGraphApprox(Graph graph)
        {
            Graph copyGraph = graph.CopyGraph();
            if (!Utility.CheckIfGraphIsConsistent(copyGraph))
            {
                Console.WriteLine("niespojny graf!");
                return;
            }

            int nodesInGraph = graph.GetNodeCount();
            List<int> color = new List<int>(nodesInGraph);
            for (int j = 0; j < nodesInGraph; j++)
            {
                color.Add(0);
            }
            Node curr;

            int colorCount = 0; //bc
            int colors = 2; //b
            bool test;

            while (true)
            {
                if (colorCount > 0)              // Kombinację sprawdzamy, gdy zawiera najstarszą cyfrę
                {
                    test = true;
                    for (int j = 0; j < nodesInGraph; j++)
                    {
                        curr = graph.GetNodeList()[j];
                        foreach (int neighbor in graph.FindNode(curr.NodeNumber).Neighbors)
                        {
                            Node tmp = graph.FindNode(neighbor);
                            if (color[j] == color[tmp.NodeNumber-1]) // Testujemy pokolorowanie
                            {
                                test = false; // Zaznaczamy porażkę
                                break;        // Opuszczamy pętlę for
                            }
                        }
                        if (!test) break; // Opuszczamy pętlę for
                    }
                    if (test) break;   // Kombinacja znaleziona, kończymy pętlę główną
                }

                while (true)         // Pętla modyfikacji licznika
                {
                    int i = 0;
                    for (i = 0; i < nodesInGraph; i++)
                    {
                        color[i]++;     // Zwiększamy cyfrę
                        if (color[i] == colors - 1) colorCount++;
                        if (color[i] < colors) break;
                        color[i] = 0;    // Zerujemy cyfrę
                        colorCount--;
                    }

                    if (i < nodesInGraph) break; // Wychodzimy z pętli zwiększania licznika
                    colors++;               // Licznik się przewinął, zwiększamy bazę
                }
            }

            for (int i = 0; i < nodesInGraph; i++)
            {
                Console.WriteLine((i + 1) + " color: " + color[i]);
            }
        }
    }
}
