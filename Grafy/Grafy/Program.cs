using System;

namespace Grafy
{
    class Program
    {
        public static string fileName = "graf1.txt";
        static void Main(string[] args)
        {
            /*Graph graph = new Graph();
            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(3);

            graph.AddNeighbour(1, 3);

            graph.ShowGraph();
            graph.SaveFile(fileName);*/

            Graph graph1 = new Graph();
            graph1.ReadFile(fileName);
            graph1.ShowGraph();
        }
    }
}
