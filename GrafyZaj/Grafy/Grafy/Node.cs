using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Grafy
{
    class Node
    {
        public int Value;
        public List<int> Neighbors;
        public Dictionary<int, int> EdgeValues;

        public Node(int _value)
        {
            Value = _value;
            Neighbors = new List<int>();
            EdgeValues = new Dictionary<int, int>();
        }

        public void SetValue(int _newVal)
        {
            Value = _newVal;
        }

        public int GetValue()
        {
            return Value;
        }

        public void AddNeighbor(Node _neighbor, int _edgeValue)
        {
            bool alreadyANeighbor = false;
            foreach (int node in Neighbors)
            {
                if (_neighbor.GetValue() == node)
                {
                    Console.WriteLine("Already a neighbor");
                    alreadyANeighbor = true;
                    break;
                }
            }

            if (!alreadyANeighbor)
            {
                Neighbors.Add(_neighbor.GetValue());
                EdgeValues.Add(_neighbor.GetValue(), _edgeValue);
            }
        }

        public void RemoveNeighbor(Node _neighbor)
        {
            foreach (int node in Neighbors)
            {
                if (_neighbor.GetValue() == node)
                {
                    Neighbors.Remove(_neighbor.GetValue());
                    EdgeValues.Remove(_neighbor.GetValue());
                    break;
                }
            }
        }

        public List<int> GetNeighbors()
        {
            return Neighbors;
        }

        /*public int GetNeighborValue()
        {
            int NeighbourValue = 0;
            foreach (Node node in neighbours)
            {
                NeighbourValue = node.GetValue();
                break;
            }

            if (NeighbourValue == 0) Console.WriteLine("Neighbor does not exist\n");

            return NeighbourValue;
        }*/

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
            //Console.WriteLine("wierzchołek_początkowy wierzchołek_końcowy waga_krawędzi");
            foreach (int neighbor in Neighbors)
            {
                Console.WriteLine(Value + " " + neighbor + " " + EdgeValues[neighbor]);
            }
        }

        public string SaveFile()
        {
            string edges = "";
            foreach (int neighbor in Neighbors)
            {
                edges += Value + " " + neighbor + " " + EdgeValues[neighbor] + "\n";
            }

            return edges;
        }
    }
}

