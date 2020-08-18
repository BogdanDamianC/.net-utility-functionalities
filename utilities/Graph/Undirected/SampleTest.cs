using System;
using System.Collections.Generic;
using System.IO;

namespace Utilities.Graph.Undirected
{
    public class SampleTest
    {
        public class GStr : GraphNode<string>
        {
            public GStr(string val) { this.Val = val; }
            public override string ToString() => Val;
        }

        public static void Test()
        {
            var (start, end) = GetSampleGraph(true);
            Test(start, end);
            Test(end, start);
            (start, end) = GetSampleGraph(false);
            Test(start, end);
            Test(end, start);
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
                getNode(ei[0]).AddEdge(getNode(ei[1]));
            }
            Test(getNode("A"), getNode("H"));
        }

        private static void Test(GStr start, GStr end)
        {
            var foundPaths = DepthFirstSearch.Execute(start, end);
            Console.WriteLine($" \r\n Finding Paths between {start} and {end} ---------------");
            foreach (var fp in foundPaths)
            {
                Console.Write(" \r\n Path: ");
                foreach (var node in fp)
                    Console.Write($" - {node}");
            }
        }


        private static (GStr, GStr) GetSampleGraph(bool useBiggerGraph)
        {
            var start = new GStr("Start");

            start.AddEdge(new GStr("g1")
                    .AddEdge(new GStr("g11")
                        .AddEdge(new GStr("g111"))
                        .AddEdge(new GStr("g112")))
                    .AddEdge(new GStr("g12")
                        .AddEdge(new GStr("g121"))
                        .AddEdge(new GStr("g122"))));
            var endNode = (GStr)start.Edges[0].Edges[0].Edges[0];

            start.AddEdge(new GStr("g2")
                    .AddEdge(new GStr("g21")
                        .AddEdge(new GStr("g211"))
                        .AddEdge(new GStr("g212")))
                    .AddEdge(new GStr("g22")
                        .AddEdge(new GStr("g221"))
                        .AddEdge(new GStr("g222"))));
            start.AddEdge(new GStr("b1")
                    .AddEdge(new GStr("b11")
                        .AddEdge(new GStr("b111"))
                        .AddEdge(new GStr("b112")))
                    .AddEdge(new GStr("b12")
                        .AddEdge(new GStr("b121"))
                        .AddEdge(new GStr("b122"))));
            var b2 = new GStr("b2")
                    .AddEdge(new GStr("b21")
                        .AddEdge(new GStr("b211"))
                        .AddEdge(new GStr("b212")))
                    .AddEdge(new GStr("b22")
                        .AddEdge(new GStr("b221"))
                        .AddEdge(new GStr("b222").AddEdge(endNode)));
            start.AddEdge(b2);
            start.AddEdge(new GStr("c1")
                    .AddEdge(new GStr("c11")
                        .AddEdge(new GStr("c111"))
                        .AddEdge(new GStr("c112")))
                    .AddEdge(new GStr("c12")
                        .AddEdge(new GStr("c121"))
                        .AddEdge(new GStr("b122"))));
            start.AddEdge(new GStr("c2")
                    .AddEdge(new GStr("c21")
                        .AddEdge(new GStr("c211"))
                        .AddEdge(new GStr("c212")))
                    .AddEdge(new GStr("c22")
                        .AddEdge(new GStr("c221"))
                        .AddEdge(new GStr("c222").AddEdge(start.Edges[0].Edges[0]))));
            start.AddEdge(new GStr("d1")
                    .AddEdge(new GStr("d11")
                        .AddEdge(start.Edges[0])
                        .AddEdge(new GStr("d111"))
                        .AddEdge(new GStr("d112")))
                    .AddEdge(new GStr("d12")
                        .AddEdge(new GStr("c121"))
                        .AddEdge(start)
                        .AddEdge(new GStr("b122"))));
            var d2 = new GStr("d2")
                    .AddEdge(new GStr("d21")
                        .AddEdge(new GStr("d211"))
                        .AddEdge(new GStr("d212")))
                    .AddEdge(new GStr("d22")
                        .AddEdge(new GStr("d221").AddEdge(start))
                        .AddEdge(new GStr("d222").AddEdge(start.Edges[0])));
            start.AddEdge(d2);
            start.AddEdge(new GStr("e2")
                    .AddEdge(new GStr("e21")
                        .AddEdge(new GStr("e211"))
                        .AddEdge(new GStr("e212").AddEdge(d2).AddEdge(b2).AddEdge(endNode))
                    .AddEdge(new GStr("e22")
                        .AddEdge(new GStr("e221").AddEdge(start))
                        .AddEdge(new GStr("e222").AddEdge(start.Edges[0])))));
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
                        currentNode.AddEdge(node);
                        currentNode = node;
                        if (i == middle)
                        {
                            if (prevMidNode != null)
                                node.AddEdge(prevMidNode);
                            prevMidNode = node;
                        }
                        i++;
                    }
                    currentNode.AddEdge(endNode);
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
