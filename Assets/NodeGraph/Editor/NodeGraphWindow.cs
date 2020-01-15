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

        var nodeView = new IntNodeView();
        nodeView.Initialize(new IntNode());

        var inputPort = new PortView(Orientation.Horizontal, Direction.Input, Capacity.Single, typeof(int), "Input");
        var outputPort = new PortView(Orientation.Horizontal, Direction.Output, Capacity.Single, typeof(int), "Output");

        nodeView.inputContainer.Add(inputPort);
        nodeView.outputContainer.Add(outputPort);

        nodeGraphView.Add(nodeView);

        nodeGraphView.StretchToParentSize();

        root.Add(nodeGraphView);
    }
}