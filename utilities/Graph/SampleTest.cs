using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utilities.Graph
{
    public class SampleTest
    {
        public class GStr : GraphNode<string, int>
        {
            public GStr(string val) { this.Val = val; }
            public override string ToString() => Val;
        }

        public static void Test()
        {
            /*var (start, end) = GetSampleGraph(true);
            Test(start, end);
            Test(end, start);*/
            var (start, end) = GetSampleGraph(false);
            Test(start, end);
            //Test(end, start);
            TestFromStream();
        }

        public static void TestFromStream()
        {
            var sr = new StreamReader(GetSampleStream());
            string edgeInfo;
            var t = new Dictionary<string, GStr>();
            Func<string, GStr> getNode = (string key) =>
            {
                if(!t.TryGetValue(key, out var node))
                {
                    node = new GStr(key);
                    t.Add(key, node);
                }
                return node;
            };
            while((edgeInfo = sr.ReadLine()) != null)
            {
                var ei = edgeInfo.Split(' ');
                getNode(ei[0]).AddEdge(1, getNode(ei[1]));
            }
            Test(getNode("A"), getNode("H"));
        }

        private static void DisplayFoundPath(List<GraphNode<string, int>> fp)
        {
            Console.Write(" \r\n Path: ");
            foreach (var node in fp)
                Console.Write($" - {node}");
        }

        private static void Test(GStr start, GStr end)
        {
            Console.WriteLine($" \r\n Finding Paths between {start} and {end} using DFS --------------- \r\n ");
            DepthFirstSearch.Execute(start, end, DisplayFoundPath);
            /**/

            Console.WriteLine($" \r\n\r\n Finding Paths between {start} and {end} using BFS ---------------  ");
            BreadthFirstSearch.Execute(start, end, DisplayFoundPath);

        }


        private static (GStr, GStr) GetSampleGraph(bool useBiggerGraph)
        {
            var start = new GStr("Start");

            start.AddEdge(1, new GStr("g1")
                    .AddEdge(1, new GStr("g11")
                        .AddEdge(1, new GStr("g111"))
                        .AddEdge(1, new GStr("g112")))
                    .AddEdge(1, new GStr("g12")
                        .AddEdge(1, new GStr("g121"))
                        .AddEdge(1, new GStr("g122"))));
            var endNode = (GStr)start.Edges.FirstOrDefault()?.Node.Edges.FirstOrDefault()?.Node.Edges.FirstOrDefault().Node;

            start.AddEdge(1, new GStr("g2")
                    .AddEdge(1, new GStr("g21")
                        .AddEdge(1, new GStr("g211"))
                        .AddEdge(1, new GStr("g212")))
                    .AddEdge(1, new GStr("g22")
                        .AddEdge(1, new GStr("g221"))
                        .AddEdge(1, new GStr("g222"))));
            start.AddEdge(1, new GStr("b1")
                    .AddEdge(1, new GStr("b11")
                        .AddEdge(1, new GStr("b111"))
                        .AddEdge(1, new GStr("b112")))
                    .AddEdge(1, new GStr("b12")
                        .AddEdge(1, new GStr("b121"))
                        .AddEdge(1, new GStr("b122"))));
            var b2 = new GStr("b2")
                    .AddEdge(1, new GStr("b21")
                        .AddEdge(1, new GStr("b211"))
                        .AddEdge(1, new GStr("b212")))
                    .AddEdge(1, new GStr("b22")
                        .AddEdge(1, new GStr("b221"))
                        .AddEdge(1, new GStr("b222").AddEdge(1, endNode)));
            start.AddEdge(1, b2);
            start.AddEdge(1, new GStr("c1")
                    .AddEdge(1, new GStr("c11")
                        .AddEdge(1, new GStr("c111"))
                        .AddEdge(1, new GStr("c112")))
                    .AddEdge(1, new GStr("c12")
                        .AddEdge(1, new GStr("c121"))
                        .AddEdge(1, new GStr("b122"))));
            start.AddEdge(1, new GStr("c2")
                    .AddEdge(1, new GStr("c21")
                        .AddEdge(1, new GStr("c211"))
                        .AddEdge(1, new GStr("c212")))
                    .AddEdge(1, new GStr("c22")
                        .AddEdge(1, new GStr("c221"))
                        .AddEdge(1, new GStr("c222").AddEdge(1, start.Edges.FirstOrDefault().Node.Edges.FirstOrDefault().Node))));
            start.AddEdge(1, new GStr("d1")
                    .AddEdge(1, new GStr("d11")
                        .AddEdge(1, start.Edges.FirstOrDefault().Node)
                        .AddEdge(1, new GStr("d111"))
                        .AddEdge(1, new GStr("d112")))
                    .AddEdge(1, new GStr("d12")
                        .AddEdge(1, new GStr("c121"))
                        .AddEdge(1, start)
                        .AddEdge(1, new GStr("b122"))));
            var d2 = new GStr("d2")
                    .AddEdge(1, new GStr("d21")
                        .AddEdge(1, new GStr("d211"))
                        .AddEdge(1, new GStr("d212")))
                    .AddEdge(1, new GStr("d22")
                        .AddEdge(1, new GStr("d221").AddEdge(1, start))
                        .AddEdge(1, new GStr("d222").AddEdge(1, start.Edges.FirstOrDefault().Node)));
            start.AddEdge(1, d2);
            start.AddEdge(1, new GStr("e2")
                    .AddEdge(1, new GStr("e21")
                        .AddEdge(1, new GStr("e211"))
                        .AddEdge(1, new GStr("e212").AddEdge(1, d2).AddEdge(1, b2).AddEdge(1, endNode))
                    .AddEdge(1, new GStr("e22")
                        .AddEdge(1, new GStr("e221").AddEdge(1, start))
                        .AddEdge(1, new GStr("e222").AddEdge(1, start.Edges.FirstOrDefault().Node)))));
            if (useBiggerGraph)
            {
                int maxNumberOfNodes = 100;
                int MaxNumberOfPathsToGenerate = 20;
                var rnd = new Random();
                GStr prevMidNode = null;
                for (int gCount = 0; gCount < MaxNumberOfPathsToGenerate; gCount++)
                {
                    var currentNode = start;                    
                    int middle = rnd.Next(1, maxNumberOfNodes - 1);//randomly link the current path to the previous path
                    int i = 0;
                    while (i < maxNumberOfNodes)
                    {
                        var node = new GStr($"long-{gCount}-{i}");
                        currentNode.AddEdge(1, node);
                        currentNode = node;
                        if (i == middle)
                        {
                            if (prevMidNode != null)
                                node.AddEdge(1, prevMidNode);
                            prevMidNode = node;
                        }
                        i++;
                    }
                    currentNode.AddEdge(1, endNode);
                }                
            }
            return (start, endNode);
        }


        private static Stream GetSampleStream()
        {
            var ms = new MemoryStream();
            var xx = new StreamWriter(ms);
            xx.WriteLine("A B");
            xx.WriteLine("B C");
            xx.WriteLine("C D");
            xx.WriteLine("D H");

            xx.WriteLine("A E");
            xx.WriteLine("E F");
            xx.WriteLine("F G");
            xx.WriteLine("G H");

            xx.WriteLine("B G");
            xx.WriteLine("E D");
            xx.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}
