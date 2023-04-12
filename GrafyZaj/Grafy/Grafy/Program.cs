using System;

namespace Grafy
{
    class Program
    {
        public static string fileName = "euler-no.txt";
        public static string fileName2 = "data_standard_out.txt";
        static void Main(string[] args)
        {
            Graph graph1 = new Graph();
            graph1.ReadFile(fileName);
            graph1.ShowGraphByNodes();
            //Euler.EulerMethod(graph1);
            //MaximumAssociation.MaxAssociationAlgorithm(graph1);
            HungarianAlgorith.HungarianAlgorithm(graph1);
        }
    }
}
