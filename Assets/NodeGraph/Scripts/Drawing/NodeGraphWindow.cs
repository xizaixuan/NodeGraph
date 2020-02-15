using ModifierNodeGraph;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

namespace ModifierNodeGraph
{
    public class NodeGraphWindow : EditorWindow
    {
        [SerializeField]
        string m_Selected;

        [SerializeField]
        GraphObject m_GraphObject;

        GraphEditorView m_GraphEditorView;

        GraphEditorView graphEditorView
        {
            get { return m_GraphEditorView; }
            set
            {
                if (m_GraphEditorView != null)
                {
                    m_GraphEditorView.RemoveFromHierarchy();
                    m_GraphEditorView.Dispose();
                }

                m_GraphEditorView = value;
                if (m_GraphEditorView != null)
                {
                    this.GetRootVisualContainer().Add(graphEditorView);
                }
            }
        }

        GraphObject graphObject
        {
            get { return m_GraphObject; }
            set
            {
                if (m_GraphObject != null)
                    DestroyImmediate(m_GraphObject);
                m_GraphObject = value;
            }
        }

        public string selectedGuid
        {
            get { return m_Selected; }
            private set { m_Selected = value; }
        }

        public NodeGraph Graph = null;

        public void Initialize(string assetGuid)
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(assetGuid));
            if (asset == null)
                return;

            if (!EditorUtility.IsPersistent(asset))
                return;

            if (selectedGuid == assetGuid)
                return;

            var path = AssetDatabase.GetAssetPath(asset);
            var extension = Path.GetExtension(path);
            if (extension == null)
                return;

            // Path.GetExtension returns the extension prefixed with ".", so we remove it. We force lower case such that
            // the comparison will be case-insensitive.
            extension = extension.Substring(1).ToLowerInvariant();
            System.Type graphType;
            switch (extension)
            {
                case ModifierGraphImporter.Extension:
                    graphType = typeof(NodeGraph);
                    break;
                default:
                    return;
            }

            selectedGuid = assetGuid;

            var textGraph = File.ReadAllText(path, Encoding.UTF8);
            graphObject = CreateInstance<GraphObject>();
            graphObject.hideFlags = HideFlags.HideAndDontSave;
            graphObject.graph = JsonUtility.FromJson(textGraph, graphType) as IGraph;

            graphEditorView = new GraphEditorView(this, m_GraphObject.graph as NodeGraph)
            {
                persistenceKey = selectedGuid,
            };

            Repaint();
        }

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
}