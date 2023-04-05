using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    public class HungarianAlgorith
    {
        public static void HungarianAlgorithm(Graph graph) 
        {
            Graph copyGraph = graph.CopyGraph();

            if(Utility.CheckIfGraphIsBiparted(copyGraph, true))
            {
                copyGraph.ShowGraphByNodes();
            }

            Graph copyGraphAlg = graph.CopyGraph();
            List<Node> copyGraphAlgList = copyGraphAlg.GetNodeList();

            for (int i = 0;i < graph.GetNodeCount(); i++)
            {
                if (copyGraph.GetNodeList()[i].Value == 1)
                {
                    int max = 0;
                    foreach (KeyValuePair<int, int> pair in copyGraphAlgList[i].EdgeValues)
                    {
                        if(pair.Value > max)
                        {
                            max = pair.Value;
                        }
                        copyGraphAlgList[i].Value = max;
                    }
                }
            }
            Console.WriteLine("Po dodaniu etykier poczatkowych");
            copyGraphAlg.ShowGraphByNodes();
        }
    }
}
