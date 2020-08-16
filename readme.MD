Depth First Search


Example 
Define your own class:
        public class YourOwnNodeType : 
        {
            public GStr(string val) { this.Val = val; }
            public override string ToString() => Val;
        }
            var start = get your own GraphNode<yourtyype>
            var end =  = get your own GraphNode<<yourtyype>
            
            var foundPaths = DepthFirstSearch.Execute(start, end);
            foreach (var fp in foundPaths)
            {
                Console.Write(" \r\n Path: ");
                foreach (var node in fp)
                    Console.Write($" - {node}");
            }
