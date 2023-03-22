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
        public static Node SelectRandomPoint(Graph graph)
        {
            var random = new Random();
            Node choosenNode = graph.GetNodeList()[random.Next(graph.GetNodeCount())];
            return choosenNode;
        }
        public static void EulerMethod(Graph graph)
        {
            if (!Utility.CheckIfGraphIsConsistent(graph)) 
            { 
                Console.WriteLine("niespojny graf!");
                return;
            }

            if(graph.GetIfGraphIsDirected()) 
            {
                Console.WriteLine("skierowany i spojny");
                foreach (Node node in graph.GetNodeList())
                {
                    if(node.NeighborsIn.Count != node.NeighborsOut.Count)
                    {
                        Console.WriteLine("Wierzcholek " + node.Index + " Ma inna liczbe krawedzi wchodzacych i wychodzacych!");
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("nieskierowany i spojny");
                foreach(Node node in graph.GetNodeList())
                {
                    if(node.NeighborsIn.Count % 2 != 0)
                    {
                        Console.WriteLine("Nieparzysta liczba krawedzi z wierzcholka " + node.Index + "! Graf nie spelnia warunkow.");
                        return;
                    }
                }
            }

            Graph copyGraph = graph.CopyGraph();
            Node current = SelectRandomPoint(copyGraph);
            Stack<Node> stack = new Stack<Node>();
            Stack<Node> cycle = new Stack<Node>();
            stack.Push(current);

            while (stack.Count != 0)
            {
                if (current.NeighborsIn.Count != 0)
                {
                    Node next = copyGraph.FindNode(current.NeighborsIn.Min());
                    copyGraph.RemoveNeighbour(next.Index, current.Index);
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
                cycleResult += node.Index + " ";
            }
            Console.WriteLine("Cykl: " + cycleResult);
        }
    }
}
