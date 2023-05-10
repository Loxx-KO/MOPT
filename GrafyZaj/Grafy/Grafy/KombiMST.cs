using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    public class KombiMST
    {
        private void MST(Graph copyGraph, List<bool> visited)
        {
            Node current = Utility.SelectRandomPoint(copyGraph);
            visited[current.NodeNumber - 1] = true;

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(current);
            BinaryTree tree = new BinaryTree();

            for (int i = 0; i < copyGraph.GetNodeCount(); i++)
            {
                current = queue.First();
                queue.Dequeue();

                foreach (int neighbor in copyGraph.FindNode(current.NodeNumber).Neighbors)
                {
                    Node tmp = copyGraph.FindNode(neighbor);
                    if (visited[tmp.NodeNumber - 1] == false)
                    {
                        visited[tmp.NodeNumber - 1] = true;
                    }
                }

                if (!visited[current.NodeNumber-1])
                {

                }
            }
        }
        public static void MST_Kombi(Graph graph)
        {
            Graph copyGraph = graph.CopyGraph();
            List<bool> visited = new List<bool>();
            for (int j = 0; j < copyGraph.GetNodeCount(); j++)
            {
                visited.Add(false);
            }
            Dictionary<int,int> path = new Dictionary<int,int>();

        }
    }
}
