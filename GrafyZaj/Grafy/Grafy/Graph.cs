using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Grafy
{
    class Graph
    {
        private List<Node> nodes;
        private bool twoSidedNeighbour;

        public Graph()
        {
            nodes = new List<Node>();
        }

        public Graph(bool _twoSideNeighbours)
        {
            nodes = new List<Node>();
            twoSidedNeighbour = _twoSideNeighbours;
        }

        public void AddNode(int _value)
        {
            if(nodes.Count == 0)
            {
                nodes.Add(new Node(_value));
            }
            else if (FindNode(_value) == null)
            {
                nodes.Add(new Node(_value));
            }
        }

        public void RemoveNode(int _value)
        {
            foreach (Node node in nodes)
            {
                if (_value == node.GetValue()) { nodes.Remove(node); break; }
            }
        }

        public Node FindNode(int _value)
        {
            foreach (Node node in nodes)
            {
                if (_value == node.GetValue()) return node;
            }
            return null;
        }

        public void AddNeighbour(int _nodeValue, int _neighbour, int _edgeValue = 0)
        {
            Node neighborNode = FindNode(_neighbour);
            if (neighborNode != null && FindNode(_nodeValue) != null)
            {
                if (twoSidedNeighbour)
                {
                    nodes[_nodeValue - 1].AddNeighbor(neighborNode, _edgeValue);
                    nodes[neighborNode.GetValue() - 1].AddNeighbor(nodes[_nodeValue - 1], _edgeValue);
                }
                else
                {
                    nodes[_nodeValue - 1].AddNeighbor(neighborNode, _edgeValue);
                }
            }
        }

        public void RemoveNeighbour(int _nodeValue, int _neighbour)
        {
            Node neighborNode = FindNode(_neighbour);
            if (neighborNode != null && FindNode(_nodeValue) != null)
            {
                if (twoSidedNeighbour)
                {
                    nodes[_nodeValue - 1].RemoveNeighbor(neighborNode);
                    nodes[neighborNode.GetValue() - 1].RemoveNeighbor(nodes[_nodeValue - 1]);
                }
                else
                {
                    nodes[_nodeValue - 1].RemoveNeighbor(neighborNode);
                }
            }
        }

        public void ShowGraph()
        {
            Console.WriteLine("#DIGRAPH");
            Console.WriteLine(twoSidedNeighbour);
            Console.WriteLine("#EDGES");

            foreach (Node node in nodes)
            {
                node.ShowContents();
            }
        }

        public void ReadFile(string filepath)
        {
            int cnt = 0;
            foreach (string line in System.IO.File.ReadLines(filepath))
            {
                if (cnt != 0 && cnt != 2)
                {
                    if (cnt == 1)
                    {
                        if (line == "false") twoSidedNeighbour = false;
                        else if (line == "true") twoSidedNeighbour = true;
                    }
                    else 
                    {
                        string[] values = line.Split(" ");
                        int begin = int.Parse(values[0]);
                        int end = int.Parse(values[1]);
                        int edge = int.Parse(values[2]);

                        AddNode(begin);
                        AddNode(end);
                        AddNeighbour(begin, end, edge);
                    }
                }
                cnt++;
            }
        }

        public void SaveFile(string filepath)
        {
            string text = "#DIGRAPH\n";
            text += twoSidedNeighbour.ToString() + "\n#EDGES\n";
            
            foreach(Node node in nodes)
            {
                text += node.SaveFile();
            }
            
            File.WriteAllText(filepath, text);
        }
    }
}

