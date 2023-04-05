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

        public bool IsGraphIsDirected()
        {
            return directed;
        }

        public void AddNode(int _nodeNumber)
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new Node(_nodeNumber));
            }
            else if (FindNode(_nodeNumber) == null)
            {
                nodes.Add(new Node(_nodeNumber));
            }
        }

        public void RemoveNode(int _nodeNumber)
        {
            foreach (Node node in nodes)
            {
                if (_nodeNumber == node.NodeNumber) { nodes.Remove(node); break; }
            }
        }

        public Node FindNode(int _nodeNumber)
        {
            foreach (Node node in nodes)
            {
                if (_nodeNumber == node.NodeNumber) return node;
            }
            return null;
        }

        public void AddNeighbor(int _nodeIndex, int _neighbour, int _edgeValue = 0)
        {
            Node neighborNode = FindNode(_neighbour);
            if (neighborNode != null && FindNode(_nodeIndex) != null)
            {
                if (!directed)
                {
                    nodes[_nodeIndex - 1].AddNeighbor(neighborNode, _edgeValue);
                    nodes[neighborNode.NodeNumber - 1].AddNeighbor(nodes[_nodeIndex - 1], _edgeValue);
                    nodes[_nodeIndex - 1].NumberOfNodesPointingToThisNode++;
                    nodes[neighborNode.NodeNumber - 1].NumberOfNodesPointingToThisNode++;
                }
                else
                {
                    nodes[_nodeIndex - 1].AddNeighbor(neighborNode, _edgeValue);
                    nodes[neighborNode.NodeNumber - 1].NumberOfNodesPointingToThisNode++;
                }
            }
        }

        public void RemoveNeighbor(int _nodeValue, int _neighbour)
        {
            Node neighborNode = FindNode(_neighbour);
            if (neighborNode != null && FindNode(_nodeValue) != null)
            {
                if (!directed)
                {
                    nodes[_nodeValue - 1].RemoveNeighbor(neighborNode);
                    nodes[neighborNode.NodeNumber - 1].RemoveNeighbor(nodes[_nodeValue - 1]);
                }
                else
                {
                    nodes[_nodeValue - 1].RemoveNeighbor(neighborNode);
                }
            }
        }

        public Graph CopyGraph()
        {
            Graph copy = CopyGraphNodes();
            for (int i = 0; i < nodes.Count; i++)
            {
                copy.nodes[i].Neighbors = new List<int>(nodes[i].Neighbors);
                copy.nodes[i].NumberOfNodesPointingToThisNode = nodes[i].NumberOfNodesPointingToThisNode;
                copy.nodes[i].EdgeValues = new Dictionary<int, int>(nodes[i].EdgeValues);
            }

            return copy;
        }

        public Graph CopyGraphNodes()
        {
            Graph copy = new Graph(directed);
            foreach (Node node in nodes)
            {
                copy.AddNode(node.NodeNumber);
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
                Console.WriteLine("Node: " + node.NodeNumber + " # Neighbors: " + node.GetNeighborsValues() + " Value: " + node.Value);
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

                        AddNode(begin);
                        AddNode(end);
                    }
                }
                cnt++;
            }

            nodes = nodes.OrderBy(x => x.NodeNumber).ToList();

            cnt = 0;
            foreach (string line in System.IO.File.ReadLines(filepath))
            {
                if (cnt > 2)
                {
                    string[] values = line.Split(" ");
                    int begin = int.Parse(values[0]);
                    int end = int.Parse(values[1]);
                    int edge = int.Parse(values[2]);

                    AddNeighbor(begin, end, edge);
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

