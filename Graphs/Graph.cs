using System.Collections.Generic;

namespace Deterministic.Graphs
{
    public class Graph
    {

        protected readonly List<Node> Nodes;
        protected Node FirstNode;

        public Graph()
        {
            Nodes = new List<Node>();
        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);
        }

        public void SetFirstNode(Node firstNode)
        {
            Nodes.Add(firstNode);
            FirstNode = firstNode;
        }

        public void StartGraph()
        {
            OnStartGraph();
        }

        protected virtual void OnStartGraph()
        {
            FirstNode.Execute();
        }
    }
}