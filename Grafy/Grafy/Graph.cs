using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Grafy
{
    class Graph
    {
        public List<Node> Nodes;

        public Graph()
        {
            Nodes = new List<Node>();
        }

        public void AddNode(int _value)
        {
            Nodes.Add(new Node(_value));
        }

        public void RemoveNode(int _value)
        {
            foreach (Node node in Nodes)
            {
                if (_value == node.GetValue()) { Nodes.Remove(node); break; }
            }
        }

        public Node FindNode(int _value)
        {
            foreach (Node node in Nodes)
            {
                if (_value == node.GetValue()) return node;
            }
            return null;
        }

        public void AddNeighbour(int _nodeValue, int _neighbour)
        {
            Node neighborNode = FindNode(_neighbour);
            if (neighborNode != null)
            {
                Nodes[_nodeValue - 1].AddNeighbor(neighborNode);
                Nodes[neighborNode.GetValue() - 1].AddNeighbor(Nodes[_nodeValue - 1]);
            }
        }

        public void ShowGraph()
        {
            foreach(Node node in Nodes)
            {
                Console.WriteLine("Node: " + node.GetValue() + " # Neighbors: " + node.GetNeighborsValues());
            }
        }

        public void ReadFile(string filepath)
        {
            string jsonString = File.ReadAllText(filepath);
            Console.WriteLine(jsonString);
            Nodes = JsonConvert.DeserializeObject<List<Node>>(jsonString)!;
        }

        public void SaveFile(string filepath)
        {
            string jsonString = JsonConvert.SerializeObject(Nodes, Formatting.Indented);
            Console.WriteLine(jsonString);
            File.WriteAllText(filepath, jsonString);
        }
    }
}
