using ModifierNodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine.Experimental.UIElements;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

public class NodeGraphWindow : EditorWindow
{
    [MenuItem("Window/NodeGraph")]
    public static void Open()
    {
        GetWindow<NodeGraphWindow>().Show();
    }

    private void OnEnable()
    {
        var root = this.GetRootVisualContainer();

        root.AddStyleSheetPath("Styles/NodeGraphView");
        
        var nodeGraphView = new NodeGraphView();
        
        nodeGraphView.AddManipulator(new SelectionDragger());
        nodeGraphView.AddManipulator(new ClickSelector());

        var node = new NodeView();

        var inputPort = new PortView(Orientation.Horizontal, Direction.Input, Capacity.Single, typeof(int), "Input");
        var outputPort = new PortView(Orientation.Horizontal, Direction.Output, Capacity.Single, typeof(int), "Output");

        node.inputContainer.Add(inputPort);
        node.outputContainer.Add(outputPort);

        nodeGraphView.Add(node);

        nodeGraphView.StretchToParentSize();

        root.Add(nodeGraphView);
    }
}