using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    class Node
    {
        public int Value;
        public List<int> Neighbors;

        public Node(int _value)
        {
            Value = _value;
            Neighbors = new List<int>();
        }

        public void SetValue(int _newVal)
        {
            Value = _newVal;
        }

        public int GetValue()
        {
            return Value;
        }

        public void AddNeighbor(Node _neighbor)
        {
            bool neighborExists = false;
            foreach (int node in Neighbors)
            {
                if (_neighbor.GetValue() == node)
                {
                    Console.WriteLine("Already a neighbor");
                    neighborExists = true;
                    break;
                }
            }

            if(!neighborExists) Neighbors.Add(_neighbor.GetValue());
        }

        public void RemoveNeighbor(Node _neighbor)
        {
            foreach (int node in Neighbors)
            {
                if (_neighbor.GetValue() == node)
                {
                    Neighbors.Remove(_neighbor.GetValue());
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
            foreach(int node in Neighbors)
            {
                if (iter != Neighbors.Count - 1) values += node + ", ";
                else values += node;

                iter++;
            }

            return values;
        }
    }
}
