using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MartinZottmann.Engine.Entities
{
    public class NodeEventArgs<T> where T : Node
    {
        public readonly NodeList<T> NodeList;

        public readonly T Node;

        public NodeEventArgs(NodeList<T> node_list, T node)
        {
            NodeList = node_list;
            Node = node;
        }
    }
}
