using System;

namespace Grafy
{
    class Program
    {
        public static string fileName = "data_standard.txt";
        public static string fileName2 = "data_standard_out.txt";
        static void Main(string[] args)
        {
            Graph graph1 = new Graph();
            graph1.ReadFile(fileName);
            graph1.ShowGraphByNodes();
            Euler.EulerMethod(graph1);
            //graph1.SaveFile(fileName2);
        }
    }
}
