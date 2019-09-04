using System.Collections.Generic;

namespace Deterministic.Graphs
{
    public class Node
    {
        protected List<Connection> InConnections;
        protected List<Connection> OutConnections;
        protected Status Status;
        protected Graph Graph;
        public Node(Graph graph)
        {
            Graph = graph;
            InConnections = new List<Connection>();
            OutConnections = new List<Connection>();
        }

        public Status Execute()
        {
            Status = OnExecute();
            return Status;
        }

        protected virtual Status OnExecute()
        {
            return Status.Success;
        }

        public void AddOutConnection(Connection connection)
        {
            OutConnections.Add(connection);
        }

        public void AddInConnection(Connection connection)
        {
            InConnections.Add(connection);
        }

    }
}