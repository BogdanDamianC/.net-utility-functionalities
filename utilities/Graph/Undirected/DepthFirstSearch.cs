using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Graph.Undirected
{
    public static class DepthFirstSearch
    {
        public static List<List<GraphNode<T>>> Execute<T>(GraphNode<T> start, GraphNode<T> end)
        {
            var context = new DFSContext<T>(end);
            context.VisitNode(start);
            return context.foundPaths;
        }

        private class DFSContext<T>
        {
            public DFSContext(GraphNode<T> endNode)
            {
                this.endNode = endNode;
            }

            private GraphNode<T> endNode;
            private Stack<GraphNode<T>> currentNodes = new Stack<GraphNode<T>>();
            public List<List<GraphNode<T>>> foundPaths = new List<List<GraphNode<T>>>();
            public void VisitNode(GraphNode<T> currentNode)
            {
                currentNode.Visited = true;
                currentNodes.Push(currentNode);
                if (currentNode == endNode)
                {
                    var paths = currentNodes.ToList();
                    paths.Reverse();
                    foundPaths.Add(paths);
                }
                else
                    foreach (var node in currentNode.Edges)
                    {
                        if (!node.Visited)
                            VisitNode(node);
                    }
                currentNodes.Pop();
                currentNode.Visited = false;
            }
        }
    }
}
