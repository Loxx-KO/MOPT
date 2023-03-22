using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    public class Utility
    {
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

                foreach(int neighbor in graph.FindNode(curr.Index).NeighborsIn)
                {
                    Node tmp = graph.FindNode(neighbor);
                    if (visited[tmp.GetNodeIndexInGraphList()] == false)
                    {
                        visited[tmp.GetNodeIndexInGraphList()] = true;
                        stack.Push(tmp);
                    }
                }
            }

            Console.WriteLine(visitedCount);

            if (visitedCount == graph.GetNodeCount()) return true;
            else return false;
        }
    }
}
