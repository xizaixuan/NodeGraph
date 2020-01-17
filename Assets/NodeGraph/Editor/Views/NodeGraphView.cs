using ModifierNodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

public class NodeGraphView : GraphView
{
    public EdgeConnectorListener ConnectorListener;

    public NodeGraphView()
    {
        InitializeManipulators();

        ConnectorListener = new EdgeConnectorListener(this);

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

        foreach (var nodeMenuItem in NodeProvider.GetNodeMenuEntries())
            evt.menu.AppendAction(nodeMenuItem.Key, (e) => CreateNodeFromType(nodeMenuItem.Value), DropdownMenu.MenuAction.AlwaysEnabled);

        base.BuildContextualMenu(evt);
    }

    private void CreateNodeFromType(Type type)
    {
        var node = Activator.CreateInstance(type) as ModifierNode;

        AddNode(node);
    }

    private void AddNode(ModifierNode node)
    {
        var viewType = NodeProvider.GetNodeViewTypeFromNodeType(node.GetType());

        if (viewType != null)
        {
            var nodeView = Activator.CreateInstance(viewType) as NodeView;
            nodeView.Initialize(this, node);
            AddElement(nodeView);
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        compatiblePorts.AddRange(ports.ToList().Where(p =>
        {
            if (p.direction == startPort.direction)
                return false;

            if (!p.portType.IsAssignableFrom(startPort.portType))
                return false;

            return true;
        }));

        return compatiblePorts;
    }
}
