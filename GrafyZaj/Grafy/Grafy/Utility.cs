using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    public class Utility
    {
        public static Node SelectRandomPoint(Graph graph)
        {
            var random = new Random();
            Node choosenNode = graph.GetNodeList()[random.Next(graph.GetNodeCount())];
            return choosenNode;
        }
        public static bool CheckIfGraphIsConsistent(Graph graph)
        {
            int nodesInGraph = graph.GetNodeCount();
            List<bool> visited = new List<bool>(nodesInGraph);
            for(int i = 0; i < nodesInGraph; i++)
            {
                visited.Add(false);
            }
            Stack<Node> stack = new Stack<Node>();
            Node curr;

            int visitedCount = 0;
            stack.Push(graph.GetNodeList()[0]);
            visited[0] = true;

            while (stack.Count != 0)
            {
                curr = stack.First();
                stack.Pop();
                visitedCount++;

                foreach(int neighbor in graph.FindNode(curr.NodeNumber).Neighbors)
                {
                    Node tmp = graph.FindNode(neighbor);
                    if (visited[tmp.NodeNumber - 1] == false)
                    {
                        visited[tmp.NodeNumber - 1] = true;
                        stack.Push(tmp);
                    }
                }
            }

            //Console.WriteLine(visitedCount);

            if (visitedCount == graph.GetNodeCount()) return true;
            else return false;
        }
        public static bool CheckIfGraphIsBiparted(Graph graph, bool addValues = false)
        {
            int nodesInGraph = graph.GetNodeCount();
            List<int> color = new List<int>(nodesInGraph);  //1 czerwony, -1 niebieski, 0 szary
            for (int i = 0; i < nodesInGraph; i++)
            {
                color.Add(0);
            }
            Queue<Node> queue = new Queue<Node>();
            Node curr;

            for (int i = 0; i < nodesInGraph; i++)
            {
                if (color[i] == 0)
                {
                    color[i] = 1;
                    queue.Enqueue(graph.GetNodeList()[i]);

                    while (queue.Count > 0)
                    {
                        curr = queue.First();
                        queue.Dequeue();

                        foreach (int neighbor in graph.FindNode(curr.NodeNumber).Neighbors)
                        {
                            Node tmp = graph.FindNode(neighbor);
                            if (color[tmp.NodeNumber - 1] == color[curr.NodeNumber - 1])
                            {
                                Console.WriteLine("Graf nie jest dwudzielny!");
                                return false;
                            }

                            if (color[tmp.NodeNumber - 1] == 0)
                            {
                                if (color[curr.NodeNumber - 1] == 1)
                                {
                                    color[tmp.NodeNumber - 1] = -1;
                                }
                                else
                                {
                                    color[tmp.NodeNumber - 1] = 1;
                                }
                                queue.Enqueue(graph.GetNodeList()[tmp.NodeNumber - 1]);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Graf jest dwudzielny!");

            if(addValues)
            {
                for (int i = 0; i < nodesInGraph; i++)
                {
                    graph.GetNodeList()[i].Value = color[i];
                }
            }

            return true;
        }
    }
}
