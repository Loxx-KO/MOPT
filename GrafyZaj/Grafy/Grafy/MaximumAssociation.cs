using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    internal class MaximumAssociation
    {
        public static void MaxAssociationAlgorithm(Graph graph) 
        {
            Graph copyGraph = graph.CopyGraph();

            if (Utility.CheckIfGraphIsBiparted(copyGraph, true))
            {
                copyGraph.ShowGraphByNodes();
            }
            else
            {
                return;
            }

            Dictionary<int, int> match = new Dictionary<int, int>();

            for (int i = 0; i < copyGraph.GetNodeCount(); i++)
            {
                match.Add((i+1),0);
            }

            int expo = copyGraph.GetNodeCount();

            /*for(int i = 0; i < copyGraph.GetNodeCount(); i++) 
            {
                if (beginMatch[i] == 0)
                {
                    foreach(int neighbor in copyGraph.GetNodeList()[i].Neighbors)
                    {
                        if (beginMatch[neighbor-1] == 0)
                        {
                            beginMatch[neighbor-1] = i+1;
                            beginMatch[i] = neighbor;
                            match.Add(i, neighbor);
                            expo -= 2;
                            break;
                        }
                    }
                }
            }*/

            for (int i = 0; i < copyGraph.GetNodeCount(); i++)
            {
                if (match[i+1] == 0)
                {
                    foreach (int neighbor in copyGraph.GetNodeList()[i].Neighbors)
                    {
                        if (match[neighbor] == 0)
                        {
                            match[i+1] = neighbor;
                            match[neighbor] = i + 1;
                            expo -= 2;
                            break;
                        }
                    }
                }
            }

            int maxAssociationCount = 0;

            List<Node> v1 = new List<Node>();
            List<Node> v2 = new List<Node>();
            List<Node> free_v1 = new List<Node>();
            List<Node> free_v2 = new List<Node>();

            Console.WriteLine("Skojarzenie poczatkowe");
            for (int i = 0; i < copyGraph.GetNodeCount(); i++)
            {
                if (copyGraph.GetNodeList()[i].Value == 1)
                {
                    Console.WriteLine(match.Keys.ToList()[i] + " --> " + match[i + 1]);

                    if (match[i + 1] != 0)
                    {
                        v1.Add(copyGraph.GetNodeList()[i]);
                        maxAssociationCount++; 
                    }
                    else free_v1.Add(copyGraph.GetNodeList()[i]);
                }
                else
                {
                    if (match[i + 1] != 0) v2.Add(copyGraph.GetNodeList()[i]);
                    else free_v2.Add(copyGraph.GetNodeList()[i]);
                }
            }

            Console.WriteLine("V1: ");
            Utility.PrintNodeList(v1);

            Console.WriteLine("V2: ");
            Utility.PrintNodeList(v2);

            Console.WriteLine("V1': ");
            Utility.PrintNodeList(free_v1);

            Console.WriteLine("V2': ");
            Utility.PrintNodeList(free_v2);

            if (free_v1.Count > 0 && free_v2.Count > 0)
            {
                List<int> path = Utility.FindPath(copyGraph, free_v1.First().NodeNumber, free_v2.First().NodeNumber);

                while (path.Count > 0)
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        if (i % 2 == 0) match[path[i]] = path[i + 1];
                    }
                    //PrintMatches(match, copyGraph);

                    free_v1.RemoveAt(0);
                    free_v2.RemoveAt(0);

                    if (free_v1.Count > 0 && free_v2.Count > 0)
                    {
                        path = Utility.FindPath(copyGraph, free_v1.First().NodeNumber, free_v2.First().NodeNumber);
                    }
                    else break;
                }
            }

            Console.WriteLine("Skojarzenie maksymalne: ");
            PrintMatches(match, copyGraph);
        }

        private static void PrintMatches(Dictionary<int, int> match, Graph copyGraph)
        {
            Console.WriteLine();
            for (int i = 0; i < copyGraph.GetNodeCount(); i++)
            {
                if (copyGraph.GetNodeList()[i].Value == 1)
                {
                    Console.WriteLine(match.Keys.ToList()[i] + " --> " + match[i + 1]);
                }
            }
        }
    }
}
