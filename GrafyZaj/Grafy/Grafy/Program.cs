using System;

namespace Grafy
{
    class Program
    {
        public static string fileName = "maxsk1.txt";
        public static string fileName2 = "colorGraph.txt";
        public static string fileName3 = "kombi_pod.txt";
        static void Main(string[] args)
        {
            Graph graph1 = new Graph();
            graph1.ReadFile(fileName);
            graph1.ShowGraphByNodes();
            //Euler.EulerMethod(graph1);
            MaximumAssociation.MaxAssociationAlgorithm(graph1);
            HungarianAlgorith.HungarianAlgorithm(graph1);

            Graph graph2 = new Graph();
            graph2.ReadFile(fileName2);
            graph2.ShowGraphByNodes();
            ColorGraph.ColorGraphApprox(graph2);

            Graph graph3 = new Graph();
            graph3.ReadFile(fileName3);
            graph3.ShowGraphByNodes();
            BranchAndBound.BranchAndBoundVersion(graph3);
        }
    }
}
