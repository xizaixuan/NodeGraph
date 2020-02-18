using ModifierNodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

using EdgeView = UnityEditor.Experimental.UIElements.GraphView.Edge;

public class NodeGraphView : GraphView
{
    public EdgeConnectorListener ConnectorListener = null;
    public NodeGraph Graph = null;

    public NodeGraphView(NodeGraph graph)
    {
        AddStyleSheetPath("Styles/NodeGraphView");

        Graph = graph;

        ConnectorListener = new EdgeConnectorListener(graph);

        this.StretchToParentSize();
    }

    public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
    {
        var compatibleAnchors = new List<Port>();
        var startSlot = startAnchor.GetSlot();
        if (startSlot == null)
            return compatibleAnchors;

        foreach (var candidateAnchor in ports.ToList())
        {
            var candidateSlot = candidateAnchor.GetSlot();
            if (!startSlot.IsCompatibleWith(candidateSlot))
                continue;

            compatibleAnchors.Add(candidateAnchor);
        }
        return compatibleAnchors;
    }
}
