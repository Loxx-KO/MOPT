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

            Graph tmp = copyGraph.CopyGraph();
            Node curr = tmp.GetNodeList()[0];
            int coloredCount = 0;
            int currentColor = 1;

            while(coloredCount < nodesInGraph) 
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
            }

            Console.WriteLine("Result: ");
            for (int i = 0; i < nodesInGraph; i++)
            {
                Console.WriteLine((i + 1) + " color: " + copyGraph.GetNodeList()[i].Value);
            }
        }
    }
}
