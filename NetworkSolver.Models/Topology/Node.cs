using System.Collections.Generic;

namespace NetworkSolver.Common.Topology
{
    public class Node
    {
        public int NodeId { get; set; }
        public List<Link> ConnectedLinks { get; set; }
        public List<Node> ConnectedNodes { get; set; }

        public Node(int id)
        {
            NodeId = id;
            ConnectedLinks = new List<Link>();
            ConnectedNodes = new List<Node>();
        }
    }
}
