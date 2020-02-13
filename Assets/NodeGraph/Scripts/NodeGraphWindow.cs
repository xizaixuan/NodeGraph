using ModifierNodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

public class NodeGraphWindow : EditorWindow
{
    public NodeGraph Graph = null;

    public static NodeGraphWindow Open()
    {
        var window = GetWindow<NodeGraphWindow>();
        window.Show();
        return window;
    }

    private void OnEnable()
    {
    }
    
    public void InitializeGraph(NodeGraph graph)
    {
        Graph = graph;

        var root = this.GetRootVisualContainer();

        var nodeGraphView = new NodeGraphView(Graph);

        nodeGraphView.AddStyleSheetPath("Styles/NodeGraphView");

        root.Add(nodeGraphView);
    }
}

[CustomEditor(typeof(NodeGraph))]
public class GraphAssetInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Graph Window"))
        {
            var window = NodeGraphWindow.Open();
            window.InitializeGraph(target as NodeGraph);
        }
    }
}