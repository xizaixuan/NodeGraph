using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;


namespace ModifierNodeGraph
{
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        readonly NodeGraphView m_Graph;

        public EdgeConnectorListener(NodeGraphView graph)
        {
            m_Graph = graph;
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        public void OnDrop(GraphView graphView, Edge edge)
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