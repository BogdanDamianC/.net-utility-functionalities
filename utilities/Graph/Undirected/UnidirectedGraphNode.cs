using System.Collections.Generic;

namespace Utilities.Graph.Undirected
{
    public class GraphNode<T>
    {
        public T Val { get; set; }
        public bool Visited { get; set; }

        public List<GraphNode<T>> Edges { get; } = new List<GraphNode<T>>();

        public X AddEdge<X>(X edge) where X : GraphNode<T>
        {
            Edges.Add(edge);
            edge.Edges.Add(this);
            return (X)this;
        }

        public void Dispose() => Edges.Clear();
    }
}
