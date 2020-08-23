using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Graph
{
    public class BreadthFirstSearch
    {
        public static void Execute<T, ET>(GraphNode<T, ET> start, GraphNode<T ,ET> end, Action<List<GraphNode<T, ET>>> onPathFound)
        {
            var startSearchTreeNode = new SearchTreeNode<T, ET>(start);
            var queue = new Queue<SearchTreeNode<T, ET>>();
            queue.Enqueue(startSearchTreeNode);
            while (queue.Any())
            {
                var stn = queue.Dequeue();
                stn.Node.Visited = true;
                if (stn.Node == end)
                    onPathFound(stn.GetPath());
                else if (stn.Node.Edges.Any())
                {
                    foreach (var edge in stn.Node.Edges)
                        if (!edge.Node.Visited)
                            queue.Enqueue(stn.AddChild(edge.Node));
                }
            }
        }

        private class SearchTreeNode<T, ET> : IDisposable
        {
            public SearchTreeNode(GraphNode<T, ET> node)
            {
                this.Node = node;
            }
            public SearchTreeNode<T, ET> AddChild(GraphNode<T, ET> edge)
            {
                var newNode = new SearchTreeNode<T, ET>(edge)
                {
                    Parent = this
                };
                Children.Add(newNode);
                return newNode;
            }

            public GraphNode<T, ET> Node { get; private set; }
            public SearchTreeNode<T, ET> Parent { get; private set; } = null;
            public List<SearchTreeNode<T, ET>> Children { get; } = new List<SearchTreeNode<T, ET>>();

            public List<GraphNode<T, ET>> GetPath()
            {
                var ret = new List<GraphNode<T, ET>>();
                var cn = this;
                while(cn != null)
                {
                    ret.Add(cn.Node);
                    cn = cn.Parent;
                }
                ret.Reverse();
                return ret;
            }

            public void Dispose()
            {
                Parent = null;
                foreach (var c in Children)
                    c.Dispose();
                Children.Clear();
                Node = null;
            }
        }
    }
}
