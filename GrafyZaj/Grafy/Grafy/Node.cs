using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Grafy
{
    public class Node
    {
        public int NodeNumber { get; private set; }
        public List<int> Neighbors;
        public Dictionary<int, int> EdgeValues;
        public int NumberOfNodesPointingToThisNode = 0;
        public int Value = 0;

        public Node(int _nodeNumber)
        {
            NodeNumber = _nodeNumber;
            Neighbors = new List<int>();
            NumberOfNodesPointingToThisNode = 0;
            EdgeValues = new Dictionary<int, int>();
        }

        public void AddNeighbor(Node _neighbor, int _edgeValue)
        {
            bool alreadyANeighbor = false;
            foreach (int node in Neighbors)
            {
                if (_neighbor.NodeNumber == node)
                {
                    //Console.WriteLine("Already a neighbor");
                    alreadyANeighbor = true;
                    break;
                }
            }

            if (!alreadyANeighbor)
            {
                Neighbors.Add(_neighbor.NodeNumber);
                EdgeValues.Add(_neighbor.NodeNumber, _edgeValue);
            }
        }

        public void RemoveNeighbor(Node _neighbor)
        {
            foreach (int node in Neighbors)
            {
                if (_neighbor.NodeNumber == node)
                {
                    Neighbors.Remove(_neighbor.NodeNumber);
                    EdgeValues.Remove(_neighbor.NodeNumber);
                    break;
                }
            }
        }

        public string GetNeighborsValues()
        {
            string values = " ";

            int iter = 0;
            foreach (int node in Neighbors)
            {
                if (iter != Neighbors.Count - 1) values += node + ", ";
                else values += node;

                iter++;
            }

            return values;
        }

        public void ShowContents()
        {
            Console.WriteLine("wierzchołek_początkowy wierzchołek_końcowy waga_krawędzi");
            foreach (int neighbor in Neighbors)
            {
                Console.WriteLine(NodeNumber + " " + neighbor + " " + EdgeValues[neighbor] + " " + Value);
            }
            Console.WriteLine();
        }

        public string SaveFile()
        {
            string edges = "";
            foreach (int neighbor in Neighbors)
            {
                edges += NodeNumber + " " + neighbor + " " + EdgeValues[neighbor] + "\n";
            }

            return edges;
        }
    }
}

