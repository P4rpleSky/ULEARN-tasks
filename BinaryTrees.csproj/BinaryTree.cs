using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        private TreeNode treeRoot;

        private class TreeNode
        {
            public T Value { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }
            public int SubtreeSize { get; set; }
        }

        public BinaryTree() { }
        
        public T this[int i]
        {
            get
            {
                var currentNode = treeRoot;
                var index = i;
                while (true)
                {
                    var leftTreeSize = currentNode.Left is null ? 
                        0 : currentNode.Left.SubtreeSize + 1;
                    if (index == leftTreeSize)
                        return currentNode.Value;
                    else if (index < leftTreeSize && !(currentNode.Left is null))
                        currentNode = currentNode.Left;
                    else
                    {
                        index -= leftTreeSize + 1;
                        currentNode = currentNode.Right;
                    }
                } 
            }
        }

        public void Add(T key)
        {
            if (treeRoot is null)
            {
                treeRoot = new TreeNode { Value = key, SubtreeSize = 0 };               
                return;
            }
            var currentNode = treeRoot;
            while (true)
            {
                currentNode.SubtreeSize += 1;
                var nextNode = key.CompareTo(currentNode.Value) < 0 ?
                    currentNode.Left : currentNode.Right;
                if (nextNode is null)
                {
                    nextNode = new TreeNode { Value = key, SubtreeSize = 0 };
                    if (nextNode.Value.CompareTo(currentNode.Value) < 0)
                        currentNode.Left = nextNode;
                    else currentNode.Right = nextNode;
                    return;
                }
                currentNode = nextNode;
            }
        }

        public bool Contains(T key)
        {
            if (treeRoot is null) return false;
            var currentNode = treeRoot;
            while (!currentNode.Value.Equals(key))
            {
                var nextNode = key.CompareTo(currentNode.Value) < 0 ?
                    currentNode.Left : currentNode.Right;
                if (nextNode is null)
                    return false;
                currentNode = nextNode;
            }
            return true;
        }
		
        public IEnumerator GetEnumerator()
        {
            var currentNode = treeRoot;
            if (currentNode is null) yield break;
            if (!(currentNode.Left is null))
            {
                treeRoot = currentNode.Left;
                var leftEnum = GetEnumerator();
                while (leftEnum.MoveNext())
                    yield return (TreeNode)leftEnum.Current;
            }
            yield return (TreeNode)currentNode;
            if (!(currentNode.Right is null))
            {
                treeRoot = currentNode.Right;
                var rightEnum = GetEnumerator();
                while (rightEnum.MoveNext())
                    yield return (TreeNode)rightEnum.Current;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var nodeEnum = GetEnumerator();
            while (nodeEnum.MoveNext())
            {
                TreeNode node = (TreeNode)nodeEnum.Current;
                yield return node.Value;
            }
        }
    }
}