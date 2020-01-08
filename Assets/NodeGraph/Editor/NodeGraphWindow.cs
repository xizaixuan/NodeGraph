using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;

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

        root.AddStyleSheetPath("Styles/Style");
        
        var nodeGraphView = new NodeGraphView();
        nodeGraphView.Add(new ModifierNode());

        nodeGraphView.StretchToParentSize();

        root.Add(nodeGraphView);
    }
}