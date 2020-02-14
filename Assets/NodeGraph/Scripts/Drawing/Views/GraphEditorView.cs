using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace ModifierNodeGraph
{
    public class GraphEditorView : VisualElement, IDisposable
    {
        NodeGraphView m_GraphView;

        NodeGraph m_Graph;

        EdgeConnectorListener m_EdgeConnectorListener;

        public NodeGraphView graphView
        {
            get { return m_GraphView; }
        }

        public GraphEditorView(EditorWindow editorWindow, NodeGraph graph)
        {
            m_Graph = graph;

            var content = new VisualElement { name = "content" };
            {
                m_GraphView = new NodeGraphView(graph) { name = "GraphView", persistenceKey = "NodeGraphView" };
                m_GraphView.AddManipulator(new ContentDragger());
                m_GraphView.AddManipulator(new SelectionDragger());
                m_GraphView.AddManipulator(new RectangleSelector());
                m_GraphView.AddManipulator(new ClickSelector());
                content.Add(m_GraphView);

                m_GraphView.graphViewChanged = GraphViewChanged;
            }

            m_EdgeConnectorListener = new EdgeConnectorListener(m_Graph);

            foreach (var node in graph.GetNodes<INode>())
                AddNode(node);

            foreach (var edge in graph.edges)
                AddEdge(edge);

            Add(content);
        }

        GraphViewChange GraphViewChanged(GraphViewChange graphViewChange)
        {
            return graphViewChange;
        }

        void AddNode(INode node)
        {
        }

        Edge AddEdge(IEdge edge)
        {
            return null;
        }


        public void Dispose()
        {
            if (m_GraphView != null)
            {
                m_GraphView = null;
            }
        }
    }
}
