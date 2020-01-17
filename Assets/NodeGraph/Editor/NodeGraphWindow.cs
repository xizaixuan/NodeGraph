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

        root.Add(nodeGraphView);
    }
}