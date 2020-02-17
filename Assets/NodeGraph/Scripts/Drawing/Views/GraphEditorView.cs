using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using EdgeView = UnityEditor.Experimental.UIElements.GraphView.Edge;

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
            AddStyleSheetPath("Styles/GraphEditorView");

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
            var nodeView = new NodeView { userData = node };
            m_GraphView.AddElement(nodeView);
            nodeView.Initialize(node as ModifierNode, m_EdgeConnectorListener);
            nodeView.MarkDirtyRepaint();
        }

        EdgeView AddEdge(IEdge edge)
        {
            var sourceNode = m_Graph.GetNodeFromGuid(edge.outputSlot.nodeGuid);
            if (sourceNode == null)
            {
                Debug.LogWarning("Source node is null");
                return null;
            }
            var sourceSlot = sourceNode.FindOutputSlot<ModifierSlot>(edge.outputSlot.slotId);

            var targetNode = m_Graph.GetNodeFromGuid(edge.inputSlot.nodeGuid);
            if (targetNode == null)
            {
                Debug.LogWarning("Target node is null");
                return null;
            }
            var targetSlot = targetNode.FindInputSlot<ModifierSlot>(edge.inputSlot.slotId);

            var sourceNodeView = m_GraphView.nodes.ToList().OfType<NodeView>().FirstOrDefault(x => x.node == sourceNode);
            if (sourceNodeView != null)
            {
                var sourceAnchor = sourceNodeView.outputContainer.Children().OfType<ModifierPort>().FirstOrDefault(x => x.slot.Equals(sourceSlot));

                var targetNodeView = m_GraphView.nodes.ToList().OfType<NodeView>().FirstOrDefault(x => x.node == targetNode);
                var targetAnchor = targetNodeView.inputContainer.Children().OfType<ModifierPort>().FirstOrDefault(x => x.slot.Equals(targetSlot));

                var edgeView = new EdgeView
                {
                    userData = edge,
                    output = sourceAnchor,
                    input = targetAnchor
                };
                edgeView.output.Connect(edgeView);
                edgeView.input.Connect(edgeView);
                m_GraphView.AddElement(edgeView);
                sourceNodeView.RefreshPorts();
                targetNodeView.RefreshPorts();

                return edgeView;
            }

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
