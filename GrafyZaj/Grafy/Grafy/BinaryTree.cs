using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafy
{
    public class TreeNode
    {
        public int Index { get; set; }
        public int Data { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public TreeNode(int data, int index)
        {
            this.Data = data;
            Left = null;
            Right = null;
            Index = index;
        }
    }
    public class BinaryTree
    {
        public TreeNode _root;
        public int Index = 0;
        public List<TreeNode> leaves = new List<TreeNode>();
        public BinaryTree()
        {
            _root = null;
            Index = 0;
        }
        public void Insert(int data)
        {
            // 1. If the tree is empty, return a new, single node 
            if (_root == null)
            {
                _root = new TreeNode(data, Index);
                return;
            }
            // 2. Otherwise, recur down the tree 
            InsertRec(_root, new TreeNode(data, Index));
            Index++;
        }
        private void InsertRec(TreeNode root, TreeNode newNode)
        {
            if (root == null)
                root = newNode;

            if (newNode.Data < root.Data)
            {
                if (root.Left == null)
                    root.Left = newNode;
                else
                    InsertRec(root.Left, newNode);

            }
            else
            {
                if (root.Right == null)
                    root.Right = newNode;
                else
                    InsertRec(root.Right, newNode);
            }
        }
        private void DisplayTree(TreeNode root)
        {
            if (root == null) return;

            DisplayTree(root.Left);
            Console.Write(root.Data + " ");
            DisplayTree(root.Right);
        }
        public List<TreeNode> GetLeaves(TreeNode root)
        {
            leaves = new List<TreeNode>();
            FindLeafNodes(root);
            return leaves;
        }
        private void FindLeafNodes(TreeNode root)
        {

            // If node is null, return
            if (root == null)
                return;

            // If node is leaf node, print its data    
            if (root.Left == null &&
                root.Right == null)
            {
                leaves.Add(root);
                return;
            }

            // If left child exists, check for leaf
            // recursively
            if (root.Left != null)
                FindLeafNodes(root.Left);

            // If right child exists, check for leaf
            // recursively
            if (root.Right != null)
                FindLeafNodes(root.Right);
        }
        public void DisplayTree()
        {
            DisplayTree(_root);
        }
        //https://www.codeguru.co.in/2015/03/how-to-create-binary-tree-in-c.html
    }
}
