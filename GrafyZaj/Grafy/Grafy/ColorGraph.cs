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
            List<int> assignList = new List<int>();
            for (int j = 0; j < nodesInGraph; j++)
            {
                color.Add(0);
                assignList.Add(0);
            }

            Graph tmp = copyGraph.CopyGraph();
            Node curr = tmp.GetNodeList()[0];
            int toBeColoredCount = 0;
            int currentColor = 1;

            List<List<int>> zbioryNiezalezne = new List<List<int>>();
            int nodesAssigned = 0;
            List<int> zbior = new List<int>();
            bool newMatchFound = false;

            while (nodesAssigned < nodesInGraph)
            {
                curr = tmp.GetNodeList()[0];
                newMatchFound = false;

                for (int i = 0; i < tmp.GetNodeCount(); i++)
                {
                    if (assignList[tmp.GetNodeList()[i].NodeNumber-1] == 0)
                    {
                        curr = tmp.GetNodeList()[i];
                        newMatchFound = true;
                        break;
                    }
                }

                if (newMatchFound)
                {
                    zbior.Add(curr.NodeNumber);
                    assignList[curr.NodeNumber - 1] = 1;

                    foreach (int neighbor in copyGraph.FindNode(curr.NodeNumber).Neighbors)
                    {
                        if (tmp.FindNode(neighbor) != null)
                        {
                            tmp.RemoveNode(neighbor);
                        }
                    }
                }
                if (!newMatchFound)
                {
                    List<int> zbiorKoncowy = zbior;
                    nodesAssigned += zbiorKoncowy.Count;
                    zbioryNiezalezne.Add(zbiorKoncowy);

                    zbior = new List<int>();
                    tmp = copyGraph.CopyGraph();
                }
            }

            toBeColoredCount = zbioryNiezalezne.Count;
            for (int i = 0; i < toBeColoredCount; i++)
            {
                List<int> maxZbior = ZnajdzMaxZbior(zbioryNiezalezne);
                if (maxZbior.Count > 0)
                {
                    foreach (int nodeNum in maxZbior)
                    {
                        copyGraph.GetNodeList()[nodeNum - 1].Value = currentColor;
                    }
                    currentColor++;
                    zbioryNiezalezne.Remove(maxZbior);
                }
            }

            /*while(coloredCount < nodesInGraph) 
            {
                curr = tmp.GetNodeList()[0];
                for (int i = 0; i < tmp.GetNodeCount(); i++)
                {
                    if (tmp.GetNodeList()[i].Value == 0)
                    {
                        curr = tmp.GetNodeList()[i];
                        break;
                    }
                }

                if (curr.Value == 0) 
                {
                    curr.Value = currentColor;
                    copyGraph.GetNodeList()[curr.NodeNumber - 1].Value = currentColor;
                    foreach (int neighbor in copyGraph.FindNode(curr.NodeNumber).Neighbors)
                    {
                        if (tmp.FindNode(neighbor) != null)
                        {
                            tmp.RemoveNode(neighbor);
                        }
                    }
                    tmp.RemoveNode(curr.NodeNumber);
                    coloredCount++;
                }
                else
                {
                    tmp.RemoveNode(curr.NodeNumber);
                }

                if (tmp.GetNodeList().Count == 0)
                {
                    currentColor++;
                    tmp = copyGraph.CopyGraph();
                }
            }*/

            Console.WriteLine("Result: ");
            for (int i = 0; i < nodesInGraph; i++)
            {
                Console.WriteLine((i + 1) + " color: " + copyGraph.GetNodeList()[i].Value);
            }
        }

        private static List<int> ZnajdzMaxZbior(List<List<int>> zbioryNiezalezne)
        {
            int max = 0;
            List<int> zbiorMax = new List<int>();
            foreach (List<int> zbior in zbioryNiezalezne)
            {
                if (zbior.Count > max)
                {
                    max = zbior.Count;
                    zbiorMax = zbior;
                }
            }

            return zbiorMax;
        }
    }
}
