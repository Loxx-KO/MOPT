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
        public int Index { get; private set; }
        public List<int> NeighborsIn;
        public List<int> NeighborsOut;
        public Dictionary<int, int> EdgeValues;

        public Node(int _index)
        {
            Index = _index;
            NeighborsIn = new List<int>();
            NeighborsOut = new List<int>();
            EdgeValues = new Dictionary<int, int>();
        }

        public int GetNodeIndexInGraphList()
        {
            return Index - 1;
        }

        public void AddNeighbor(Node _neighbor, bool directed, int _edgeValue)
        {
            bool alreadyANeighbor = false;
            foreach (int node in NeighborsIn)
            {
                if (_neighbor.Index == node)
                {
                    Console.WriteLine("Already a neighbor");
                    alreadyANeighbor = true;
                    break;
                }
            }

            if (!alreadyANeighbor)
            {
                if (!directed) AddNeighborIn(_neighbor, _edgeValue);
                else
                {
                    AddNeighborIn(_neighbor, _edgeValue);
                    AddNeighborOut(_neighbor);
                }
            }
        }

        private void AddNeighborIn(Node _neighbor, int _edgeValue)
        {
            bool alreadyANeighbor = false;
            foreach (int node in NeighborsIn)
            {
                if (_neighbor.Index == node)
                {
                    Console.WriteLine("Already a neighbor");
                    alreadyANeighbor = true;
                    break;
                }
            }

            if (!alreadyANeighbor)
            {
                NeighborsIn.Add(_neighbor.Index);
                EdgeValues.Add(_neighbor.Index, _edgeValue);
            }
        }

        private void AddNeighborOut(Node _neighbor)
        {
            bool alreadyANeighbor = false;
            foreach (int node in NeighborsIn)
            {
                if (_neighbor.Index == node)
                {
                    Console.WriteLine("Already a neighbor");
                    alreadyANeighbor = true;
                    break;
                }
            }

            if (!alreadyANeighbor)
            {
                NeighborsOut.Add(_neighbor.Index);
            }
        }

        public void RemoveNeighbor(Node _neighbor, bool directed)
        {
            if(!directed)
            {
                RemoveNeighborIn(_neighbor);
            }
            else
            {
                RemoveNeighborIn(_neighbor);
                RemoveNeighborOut(_neighbor);
            }
        }

        public void RemoveNeighborIn(Node _neighbor)
        {
            foreach (int node in NeighborsIn)
            {
                if (_neighbor.Index == node)
                {
                    NeighborsIn.Remove(_neighbor.Index);
                    EdgeValues.Remove(_neighbor.Index);
                    break;
                }
            }
        }

        public void RemoveNeighborOut(Node _neighbor)
        {
            foreach (int node in NeighborsIn)
            {
                if (_neighbor.Index == node)
                {
                    NeighborsOut.Remove(_neighbor.Index);
                    break;
                }
            }
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
            foreach (int node in NeighborsIn)
            {
                if (iter != NeighborsIn.Count - 1) values += node + ", ";
                else values += node;

                iter++;
            }

            return values;
        }

        public void ShowContents()
        {
            //Console.WriteLine("wierzchołek_początkowy wierzchołek_końcowy waga_krawędzi");
            foreach (int neighbor in NeighborsIn)
            {
                Console.WriteLine(Index + " " + neighbor + " " + EdgeValues[neighbor]);
            }
        }

        public string SaveFile()
        {
            string edges = "";
            foreach (int neighbor in NeighborsIn)
            {
                edges += Index + " " + neighbor + " " + EdgeValues[neighbor] + "\n";
            }

            return edges;
        }
    }
}

