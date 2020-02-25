using ModifierNodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class NodeGraphView : GraphView
{
    public NodeGraphView(NodeGraph graph)
    {
        AddStyleSheetPath("Styles/NodeGraphView");

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
