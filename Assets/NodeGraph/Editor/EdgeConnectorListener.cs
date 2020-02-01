using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

using EdgeView = UnityEditor.Experimental.UIElements.GraphView.Edge;

namespace ModifierNodeGraph
{
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        readonly NodeGraphView m_Graph;

        public EdgeConnectorListener(NodeGraphView graph)
        {
            m_Graph = graph;
        }

        public void OnDropOutsidePort(EdgeView edge, Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        public void OnDrop(GraphView graphView, EdgeView edge)
        {
            var leftSlot = edge.output;
            var rightSlot = edge.input;
            if (leftSlot != null && rightSlot != null)
            {
                m_Graph.AddElement(edge);
            }
        }
    }
}