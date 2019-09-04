using NUnit.Framework;

namespace Deterministic.Graphs.Tests
{
    [TestFixture]
    public class GraphTest
    {
        private Graph _graph;

        [SetUp]
        public void SetUp()
        {
            _graph = new Graph();
        }

        [Test]
        public void TestAddRemoveNode()
        {
            Node node = new Node(_graph);
            _graph.AddNode(node);
            _graph.RemoveNode(node);
        }

        [Test]
        public void TransitToAnotherNode()
        {
            Node node1 = new Node(_graph);
            Node node2 = new Node(_graph);
            Connection connection = new Connection(node1, node2, _graph);

            _graph.AddNode(node1);
            _graph.AddNode(node2);
            _graph.SetFirstNode(node1);
            _graph.StartGraph();

        }


    }
}
