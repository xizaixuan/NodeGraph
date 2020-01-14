using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
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

        root.AddStyleSheetPath("Styles/NodeGraphView");
        
        var nodeGraphView = new NodeGraphView();
        
        nodeGraphView.AddManipulator(new SelectionDragger());
        nodeGraphView.AddManipulator(new ClickSelector());

        nodeGraphView.Add(new ModifierNode());

        nodeGraphView.StretchToParentSize();

        root.Add(nodeGraphView);
    }
}