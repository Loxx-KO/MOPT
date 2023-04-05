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

            int nodesInGraph = copyGraph.GetNodeCount();
            List<Node> nodes = copyGraph.GetNodeList();
            Node curr;

            List<int> mathing = new List<int>(nodesInGraph);
            List<bool> visited = new List<bool>(nodesInGraph);
            List<int> augment = new List<int>(nodesInGraph);

            for (int i = 0; i < nodesInGraph; i++)
            {
                mathing.Add(-1);
                visited.Add(false);
                augment.Add(0);
            }
            Queue<Node> queue = new Queue<Node>();

            // kawaler niebieski = -1, panna czerwony = 1

            for (int i = 0; i < nodesInGraph; i++)
            {
                if ((mathing[i] == -1) && nodes[i].Value == 1)
                {
                    for (int j = 0; j < nodesInGraph; j++) visited[j] = false;
                    queue.Clear();

                    visited[i] = true;
                    augment[i] = -1;
                    queue.Enqueue(nodes[i]);

                    while (queue.Count != 0)
                    {
                        curr = queue.First();
                        queue.Dequeue();

                        if(curr.Value == -1) //if kawalerowie niebiescy (-1)
                        {
                            if (mathing[curr.NodeNumber - 1] == -1)  //if kawaler wolny
                            {
                                while (augment[curr.NodeNumber - 1] > -1)
                                {
                                    if (curr.Value == -1)
                                    {
                                        //zamiana krawedzi skojarzonych na nieskojarzone
                                        mathing[curr.NodeNumber - 1] = augment[curr.NodeNumber - 1];
                                        mathing[augment[curr.NodeNumber - 1]] = curr.NodeNumber-1;
                                    }
                                    curr = nodes[augment[curr.NodeNumber - 1]];
                                }
                                break;
                            }
                            else
                            {
                                // Kawaler skojarzony
                                augment[mathing[curr.NodeNumber - 1]] = curr.NodeNumber-1;
                                visited[mathing[curr.NodeNumber - 1]] = true;
                                queue.Enqueue(nodes[mathing[curr.NodeNumber - 1]]); // W kolejce umieszczamy skojarzoną pannę
                            }
                        }
                        else //panna czerwona (1)
                        {
                            foreach (int neighbor in copyGraph.FindNode(curr.NodeNumber).Neighbors)
                            {
                                Node tmp = copyGraph.FindNode(neighbor);
                                if (visited[tmp.NodeNumber - 1] == false) // kawaler nieskojarzony
                                {
                                    visited[tmp.NodeNumber - 1] = true;
                                    augment[tmp.NodeNumber - 1] = curr.NodeNumber-1;
                                    queue.Enqueue(tmp);
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Skojarzenia czerwony(panna) --- niebieski(kawaler)");
            for (int i = 0; i < nodesInGraph; i++)
            {
                if (nodes[i].Value == -1)
                {
                    Console.WriteLine("# " + (i+1) + " --> " + (mathing[i]+1));
                }
            }

            Console.WriteLine("Sciezka rozszerzajaca");
            for (int i = 0; i < nodesInGraph; i++)
            {
                if (nodes[i].Value == 1)
                {
                    Console.Write(" " + (i + 1) + " --> " + (mathing[i] + 1) + " --> ");
                }
            }
        }
    }
}
