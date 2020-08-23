using System.Collections.Generic;

namespace Utilities.Graph
{

    /// <summary>
    /// A generic node that assumes that the connection between the Vertices can go in each direction and the cost/distance of the edges is the same
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GraphNode<T, ET>
    {
        public T Val { get; set; }
        public bool Visited { get; set; }

        private List<GraphEdge<T, ET>> edges = new List<GraphEdge<T, ET>>();

        public int EdgesCount { get => edges.Count; }
        public IEnumerable<GraphEdge<T, ET>> Edges { get => edges; }

        public X AddEdge<X>(ET edgeInfo, X node) where X: GraphNode<T, ET>
        {
            edges.Add(new GraphEdge<T, ET>(edgeInfo, node));
            return (X)this;
        }

        public void Dispose() => edges.Clear();
    }

    public class GraphEdge<T, ET>
    {
        public GraphEdge(ET EdgeInfo, GraphNode<T, ET> Node)
        {
            this.EdgeInfo = EdgeInfo;
            this.Node = Node;
        }

        public ET EdgeInfo { get; private set; }
        public GraphNode<T, ET> Node { get; private set; }
    }
}
