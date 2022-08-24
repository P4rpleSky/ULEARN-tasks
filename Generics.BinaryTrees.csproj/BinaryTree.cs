using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
    public static class BinaryTree
    {
        public static BinaryTree<T1> Create<T1>(params T1[] keys) where T1 : IComparable
        {
            var result = new BinaryTree<T1>();
            foreach (var key in keys) 
                result.Add(key);
            return result;
        }
    }

    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        public T Value { get; set; }
        public BinaryTree<T> Left { get; set; }
        public BinaryTree<T> Right { get; set; }
        public int SubtreeSize { get; set; }
        public bool IsEmpty { get => this.SubtreeSize == 0; }

        public void Add(T key)
        {
            if (this.IsEmpty)
            {
                this.Value =  key;
                this.SubtreeSize = 1;
                return;
            }
            var currentNode = this;
            while (true)
            {
                currentNode.SubtreeSize += 1;
                var nextNode = key.CompareTo(currentNode.Value) <= 0 ?
                    currentNode.Left : currentNode.Right;
                if (nextNode is null)
                {
                    nextNode = new BinaryTree<T> { Value = key, SubtreeSize = 1};
                    if (nextNode.Value.CompareTo(currentNode.Value) <= 0)
                        currentNode.Left = nextNode;
                    else 
                        currentNode.Right = nextNode;
                    return;
                }
                currentNode = nextNode;
            }
        }

        public IEnumerator GetEnumerator()
        {
            if (this.IsEmpty) yield break;
            if (!(this.Left is null))
            {
                var leftEnum = this.Left.GetEnumerator();
                while (leftEnum.MoveNext())
                    yield return leftEnum.Current;
            }
            yield return this.Value;
            if (!(this.Right is null))
            {
                var rightEnum = this.Right.GetEnumerator();
                while (rightEnum.MoveNext())
                    yield return rightEnum.Current;
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
                yield return (T)enumerator.Current;
        }
    }
}