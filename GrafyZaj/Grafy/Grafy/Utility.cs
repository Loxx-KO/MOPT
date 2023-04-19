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
        public static List<int> FindPath(Graph graph, int start, int finish)
        {
            int nodesInGraph = graph.GetNodeCount();
            List<bool> visited = new List<bool>(nodesInGraph);
            List<int> path = new List<int>();

            for (int i = 0; i < nodesInGraph; i++)
            {
                visited.Add(false);
                path.Add(-1);
            }

            List<int> finishedPath = new List<int>();
            Stack<Node> stack = new Stack<Node>();
            Node curr;

            stack.Push(graph.GetNodeList()[start-1]);
            path[start - 1] = -1;
            visited[start - 1] = true;

            bool found = false;

            while (stack.Count != 0)
            {
                curr = stack.First();
                stack.Pop();

               if(curr.NodeNumber == finish)
                {
                    found = true;
                    break;
                }

                if (curr.NodeNumber != finish) 
                {
                    foreach (int neighbor in graph.FindNode(curr.NodeNumber).Neighbors)
                    {
                        Node tmp = graph.FindNode(neighbor);
                        if (visited[tmp.NodeNumber - 1] == false)
                        {
                            path[tmp.NodeNumber - 1] = curr.NodeNumber-1;
                            visited[tmp.NodeNumber - 1] = true;
                            stack.Push(tmp);
                        }
                    }
                }
            }

            if (found)
            {
                curr = graph.GetNodeList()[finish - 1];
                while (path[curr.NodeNumber - 1] > -1)
                {
                    finishedPath.Add(path[curr.NodeNumber - 1]+1);
                    curr = graph.GetNodeList()[path[curr.NodeNumber - 1]];
                }

                finishedPath.Reverse();
                finishedPath.Add(graph.GetNodeList()[finish - 1].NodeNumber);
                /*for (int i = 0; i < finishedPath.Count; i++)
                {
                    Console.Write(finishedPath[i] + " -> ");
                }*/
            }
            else Console.WriteLine("No path!");

            return finishedPath;
        }
        public static void PrintNodeList(List<Node> intList)
        {
            for (int i = 0; i < intList.Count; i++)
            {
                Console.Write(intList[i].NodeNumber + " ");
            }
            Console.WriteLine();
        }
        public static int[,] GraphTo2D_Matrix(List<Node> v1, List<Node> v2)
        {
            int[,] matrix = new int[v1.Count, v2.Count];

            for(int i = 0 ; i < v1.Count; i++)
            {
                for (int j = 0; j < v2.Count; j++)
                {
                    matrix[i, j] = 0;
                }
            }

            int it = 0;
            int minV2Value = v2[0].NodeNumber-1;
            foreach (Node node in v1) 
            {
                foreach(int neighbor in node.Neighbors) 
                {
                    matrix[node.NodeNumber-1, (neighbor-1) - v1.Count] = node.EdgeValues[neighbor];
                }
                it++;
            }

            return matrix;
        }
        public static void Show2D_Matrix(int[,] matrix, List<Node> v1, List<Node> v2)
        {
            Console.Write("# ");
            for (int j = 0; j < v2.Count; j++)
            {
                Console.Write(v2[j].NodeNumber + " ");
            }
            Console.WriteLine();

            for (int i = 0; i < v1.Count; i++)
            {
                Console.Write(v1[i].NodeNumber + " ");
                for (int j = 0; j < v2.Count; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        public static void Show2D_Matrix_Graph(int[,] matrix, Graph graph)
        {
            Console.Write("# ");
            for (int j = 0; j < graph.GetNodeCount(); j++)
            {
                Console.Write(graph.GetNodeList()[j].NodeNumber + " ");
            }
            Console.WriteLine();

            for (int i = 0; i < graph.GetNodeCount(); i++)
            {
                Console.Write(graph.GetNodeList()[i].NodeNumber + " ");
                for (int j = 0; j < graph.GetNodeCount(); j++)
                {
                    if (matrix[i,j] == int.MaxValue) { Console.Write("Inf ");  }
                    else Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
