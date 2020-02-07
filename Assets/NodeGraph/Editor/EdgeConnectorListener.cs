using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

using EdgeView = UnityEditor.Experimental.UIElements.GraphView.Edge;

namespace ModifierNodeGraph
{
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        readonly NodeGraph m_Graph;

        public EdgeConnectorListener(NodeGraph graph)
        {
            m_Graph = graph;
        }

        public void OnDrop(GraphView graphView, EdgeView edge)
        {
            var leftSlot = edge.output.GetSlot();
            var rightSlot = edge.input.GetSlot();
            if (leftSlot != null && rightSlot != null)
            {
                m_Graph.Connect(leftSlot.slotReference, rightSlot.slotReference);
                graphView.AddElement(edge);
            }
        }

        public void OnDropOutsidePort(EdgeView edge, Vector2 position)
        {
        }
    }
}