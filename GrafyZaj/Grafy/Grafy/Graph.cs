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
    public class Graph
    {
        private List<Node> nodes;
        private bool directed;

        public Graph()
        {
            nodes = new List<Node>();
        }

        public Graph(bool _directed)
        {
            nodes = new List<Node>();
            directed = _directed;
        }

        public List<Node> GetNodeList()
        {
            return nodes;
        }

        public int GetNodeCount()
        {
            return nodes.Count;
        }

        public bool GetIfGraphIsDirected()
        {
            return directed;
        }

        public void AddNode(int _index)
        {
            if(nodes.Count == 0)
            {
                nodes.Add(new Node(_index));
            }
            else if (FindNode(_index) == null)
            {
                nodes.Add(new Node(_index));
            }
        }

        public void RemoveNode(int _value)
        {
            foreach (Node node in nodes)
            {
                if (_value == node.Index) { nodes.Remove(node); break; }
            }
        }

        public Node FindNode(int _value)
        {
            foreach (Node node in nodes)
            {
                if (_value == node.Index) return node;
            }
            return null;
        }

        public void AddNeighbour(int _nodeValue, int _neighbour, int _edgeValue = 0)
        {
            Node neighborNode = FindNode(_neighbour);
            if (neighborNode != null && FindNode(_nodeValue) != null)
            {
                if (!directed)
                {
                    nodes[_nodeValue - 1].AddNeighbor(neighborNode, directed, _edgeValue);
                    nodes[neighborNode.Index - 1].AddNeighbor(nodes[_nodeValue - 1], directed, _edgeValue);
                }
                else
                {
                    nodes[_nodeValue - 1].AddNeighbor(neighborNode, directed, _edgeValue);
                    //nodes[neighborNode.Index - 1].AddNeighborOut(nodes[_nodeValue - 1]);
                }
            }
        }

        public void RemoveNeighbour(int _nodeValue, int _neighbour)
        {
            Node neighborNode = FindNode(_neighbour);
            if (neighborNode != null && FindNode(_nodeValue) != null)
            {
                if (!directed)
                {
                    nodes[_nodeValue - 1].RemoveNeighbor(neighborNode, directed);
                    nodes[neighborNode.Index - 1].RemoveNeighbor(nodes[_nodeValue - 1], directed);
                }
                else
                {
                    nodes[_nodeValue - 1].RemoveNeighbor(neighborNode, directed);
                }
            }
        }

        public Graph CopyGraph()
        {
            Graph copy = new Graph(directed);
            foreach (Node node in nodes)
            {
                copy.AddNode(node.Index);
            }
            for(int i = 0; i < nodes.Count; i++)
            {
                copy.nodes[i].NeighborsIn = new List<int>(nodes[i].NeighborsIn);
                copy.nodes[i].NeighborsOut = new List<int>(nodes[i].NeighborsOut);
                copy.nodes[i].EdgeValues = new Dictionary<int, int>(nodes[i].EdgeValues);
            }

            return copy;
        }

        public void ShowGraphByFileFormat()
        {
            Console.WriteLine("#DIGRAPH");
            Console.WriteLine(directed);
            Console.WriteLine("#EDGES");

            foreach (Node node in nodes)
            {
                node.ShowContents();
            }
        }

        public void ShowGraphByNodes()
        {
            foreach (Node node in nodes)
            {
                Console.WriteLine("Node: " + node.Index + " # Neighbors: " + node.GetNeighborsValues());
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
                        if (line == "false") directed = false;
                        else if (line == "true") directed = true;
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
            text += directed.ToString() + "\n#EDGES\n";
            
            foreach(Node node in nodes)
            {
                text += node.SaveFile();
            }
            
            File.WriteAllText(filepath, text);
        }
    }
}

