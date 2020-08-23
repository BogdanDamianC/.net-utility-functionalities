using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Graph
{
    public static class DepthFirstSearch
    {
        public static void Execute<T, ET>(GraphNode<T, ET> start, GraphNode<T,ET> end, Action<List<GraphNode<T, ET>>> onPathFound)
        {
            var context = new DFSContext<T, ET>(end, onPathFound);
            context.VisitNode(start);
        }

        private class DFSContext<T, ET>
        {
            private readonly Action<List<GraphNode<T, ET>>> onPathFound;
            private readonly GraphNode<T, ET> endNode;
            private readonly Stack<GraphNode<T, ET>> currentNodes = new Stack<GraphNode<T, ET>>();
            public DFSContext(GraphNode<T, ET> endNode, Action<List<GraphNode<T, ET>>> onPathFound)
            {
                this.endNode = endNode;
                this.onPathFound = onPathFound;
            }
            
            public void VisitNode(GraphNode<T, ET> currentNode)
            {
                currentNode.Visited = true;
                currentNodes.Push(currentNode);
                if (currentNode == endNode)
                {
                    var paths = currentNodes.ToList();
                    paths.Reverse();
                    onPathFound(paths);
                }
                else
                    foreach (var edge in currentNode.Edges)
                    {
                        if (!edge.Node.Visited)
                            VisitNode(edge.Node);
                    }
                currentNodes.Pop();
                currentNode.Visited = false;
            }
        }
    }
}
