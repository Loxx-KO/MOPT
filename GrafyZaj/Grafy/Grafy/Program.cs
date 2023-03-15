using System;

namespace Grafy
{
    class Program
    {
        public static string fileName = "data_standard.txt";
        public static string fileName2 = "data_standard_out.txt";
        static void Main(string[] args)
        {
            /*Graph graph = new Graph(true);
            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(3);

            graph.AddNeighbour(1, 3, 20);

            graph.ShowGraph();

            Graph graph2 = new Graph(false);
            graph2.AddNode(1);
            graph2.AddNode(2);
            graph2.AddNode(3);

            graph2.AddNeighbour(1, 3, 20);
            graph2.AddNeighbour(1, 2, 10);
            graph2.AddNeighbour(3, 2, 40);

            graph2.ShowGraph();*/

            Graph graph1 = new Graph();
            graph1.ReadFile(fileName);
            graph1.ShowGraph();
            graph1.SaveFile(fileName2);
        }
    }
}
