using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Iterator
{
    class Program
    {
        static void Main(string[] args)
        {
            //     1
            //    / \
            //   2   3
            var root = new Node<int>(1, new Node<int>(2), new Node<int>(3));
            var it = new InOrderIterator<int>(root);
            while(it.MoveNext())
            {
                Console.Write(it.Current.Value);
                Console.Write(',');
            }

            Console.WriteLine();

            var tree = new BinaryTree<int>(root);
            Console.WriteLine(string.Join(",", tree.InOrder.Select(x => x.Value)));
        }
    }

    #region Iterator Object
    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value)
        {
            Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right)
        {
            Value = value;
            Left = left;
            Right = right;
            left.Parent = right.Parent = this;
        }
    }

    //迭代器
    public class InOrderIterator<T>
    {
        public Node<T> Current;
        private readonly Node<T> root;
        private bool yieldedStart;

        public InOrderIterator(Node<T> root)
        {
            this.root = root;
            Current = root;
            while (Current.Left != null)
            {
                Current = Current.Left;
            }
        }

        public bool MoveNext()
        {
            if (!yieldedStart)
            {
                yieldedStart = true;
                return true;
            }

            if (Current.Right != null)
            {
                Current = Current.Right;
                while (Current.Left != null)
                    Current = Current.Left;
                return true;
            }
            else
            {
                var p = Current.Parent;
                while (p != null && Current == p.Right)
                {
                    Current = p;
                    p = p.Parent;
                }
                Current = p;
                return Current != null;
            }
        }

        public void Reset()
        {

        }
    }
    #endregion

    #region Iterator Method
    public class BinaryTree<T>
    {
        private Node<T> root;
        public BinaryTree(Node<T> root)
        {
            this.root = root;
        }

        public IEnumerable<Node<T>> InOrder
        {
            get
            {
                IEnumerable<Node<T>> Traverse(Node<T> current)
                {
                    if(current.Left!=null)
                    {
                        foreach (var left in Traverse(current.Left))
                            yield return left;
                    }
                    yield return current;
                    if(current.Right!=null)
                    {
                        foreach (var right in Traverse(current.Right))
                            yield return right;
                    }
                }
                foreach (var node in Traverse(root))
                    yield return node;
            }
        }
    }
    #endregion
}