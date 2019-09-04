namespace Deterministic.Graphs
{
    public class Connection
    {
        protected Node FromNode;
        protected Node ToNode;
        protected Graph Graph;
        protected Status Status;

        public Connection(Node fromNode, Node toNode, Graph graph)
        {
            Graph = graph;
            FromNode = fromNode;
            ToNode = toNode;

            AddThisConnectionToNodes();
        }

        private void AddThisConnectionToNodes()
        {
            FromNode.AddOutConnection(this);
            ToNode.AddInConnection(this);
        }

       
    }
}