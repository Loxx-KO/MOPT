using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    public class Euler
    {

        public static bool EulerMethod(Graph graph)
        {
            if (!Utility.CheckIfGraphIsConsistent(graph)) 
            { 
                Console.WriteLine("niespojny graf!");
                return false;
            }

            if(graph.IsGraphIsDirected()) 
            {
                //Console.WriteLine("skierowany i spojny");
                foreach (Node node in graph.GetNodeList())
                {
                    if(node.Neighbors.Count != node.NumberOfNodesPointingToThisNode)
                    {
                        Console.WriteLine("Wierzcholek " + node.NodeNumber + " Ma inna liczbe krawedzi wchodzacych i wychodzacych!");
                        return false;
                    }
                }
            }
            else
            {
                //Console.WriteLine("nieskierowany i spojny");
                foreach(Node node in graph.GetNodeList())
                {
                    if(node.Neighbors.Count % 2 != 0)
                    {
                        Console.WriteLine("Nieparzysta liczba krawedzi z wierzcholka " + node.NodeNumber + "! Graf nie spelnia warunkow.");
                        return false;
                    }
                }
            }

            Graph copyGraph = graph.CopyGraph();
            Node current = Utility.SelectRandomPoint(copyGraph);
            Stack<Node> stack = new Stack<Node>();
            Stack<Node> cycle = new Stack<Node>();
            stack.Push(current);

            while (stack.Count != 0)
            {
                if (current.Neighbors.Count != 0)
                {
                    Node next = copyGraph.FindNode(current.Neighbors.First());
                    copyGraph.RemoveNeighbor(next.NodeNumber, current.NodeNumber);
                    current = next;
                    stack.Push(current);
                }
                else
                {
                    cycle.Push(current);
                    stack.Pop();

                    if(stack.Count != 0) current = stack.First();
                }
            }

            string cycleResult = "";
            foreach(Node node in cycle)
            {
                cycleResult += node.NodeNumber + " ";
            }
            Console.WriteLine("Cykl: " + cycleResult);

            return true;
        }
    }
}
