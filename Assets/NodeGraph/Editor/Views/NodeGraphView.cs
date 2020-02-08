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
        Graph = graph;

        ConnectorListener = new EdgeConnectorListener(graph);

        InitializeManipulators();
        InitializeNodeViews();
        InitializeEdges();

        this.StretchToParentSize();
    }

    private void InitializeManipulators()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ClickSelector());
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendSeparator();

        var mousePosition = evt.mousePosition;
        foreach (var menuItem in NodeProvider.GetNodeMenuEntries())
        {
            evt.menu.AppendAction(menuItem.Key, (e) => CreateNodeFromType(menuItem.Value, mousePosition), DropdownMenu.MenuAction.AlwaysEnabled);
        }

        base.BuildContextualMenu(evt);
    }

    private void CreateNodeFromType(Type type, Vector2 position)
    {
        var node = Activator.CreateInstance(type) as ModifierNode;
        var drawState = node.drawState;
        drawState.position = new Rect(position, new Vector2(50, 50));
        node.drawState = drawState;
        Graph.AddNode(node);
        AddNodeView(node);
    }

    private void AddNodeView(ModifierNode node)
    {
        var viewType = NodeProvider.GetNodeViewTypeFromNodeType(node.GetType());

        if (viewType != null)
        {
            var nodeView = Activator.CreateInstance(viewType) as NodeView;
            nodeView.Initialize(this, node);
            AddElement(nodeView);
        }
    }

    private void InitializeNodeViews()
    {
        foreach (var node in Graph.GetNodes<ModifierNode>())
            AddNodeView(node);
    }

    private void InitializeEdges()
    {
        foreach (var edge in Graph.edges)
        {
            AddEdgeView(edge);
        }
    }

    EdgeView AddEdgeView(IEdge edge)
    {
        var sourceNode = Graph.GetNodeFromGuid(edge.outputSlot.nodeGuid);
        if (sourceNode == null)
        {
            Debug.LogWarning("Source node is null");
            return null;
        }
        var sourceSlot = sourceNode.FindOutputSlot<ModifierSlot>(edge.outputSlot.slotId);

        var targetNode = Graph.GetNodeFromGuid(edge.inputSlot.nodeGuid);
        if (targetNode == null)
        {
            Debug.LogWarning("Target node is null");
            return null;
        }
        var targetSlot = targetNode.FindInputSlot<ModifierSlot>(edge.inputSlot.slotId);

        var sourceNodeView = nodes.ToList().OfType<NodeView>().FirstOrDefault(x => x.node == sourceNode);
        if (sourceNodeView != null)
        {
            var sourceAnchor = sourceNodeView.outputContainer.Children().OfType<ModifierPort>().FirstOrDefault(x => x.slot.Equals(sourceSlot));

            var targetNodeView = nodes.ToList().OfType<NodeView>().FirstOrDefault(x => x.node == targetNode);
            var targetAnchor = targetNodeView.inputContainer.Children().OfType<ModifierPort>().FirstOrDefault(x => x.slot.Equals(targetSlot));

            var edgeView = new EdgeView
            {
                userData = edge,
                output = sourceAnchor,
                input = targetAnchor
            };
            edgeView.output.Connect(edgeView);
            edgeView.input.Connect(edgeView);
            AddElement(edgeView);
            sourceNodeView.RefreshPorts();
            targetNodeView.RefreshPorts();
            //sourceNodeView.UpdatePortInputTypes();
            //targetNodeView.UpdatePortInputTypes();

            return edgeView;
        }

        return null;
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
