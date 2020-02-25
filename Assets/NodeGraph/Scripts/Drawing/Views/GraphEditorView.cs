using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using EdgeView = UnityEditor.Experimental.UIElements.GraphView.Edge;
using Object = UnityEngine.Object;

namespace ModifierNodeGraph
{
    public class GraphEditorView : VisualElement, IDisposable
    {
        NodeGraphView m_GraphView;

        NodeGraph m_Graph;

        SearchWindowProvider m_SearchWindowProvider;

        EdgeConnectorListener m_EdgeConnectorListener;

        public Action saveRequested { get; set; }

        public NodeGraphView graphView
        {
            get { return m_GraphView; }
        }

        public GraphEditorView(EditorWindow editorWindow, NodeGraph graph)
        {
            AddStyleSheetPath("Styles/GraphEditorView");

            m_Graph = graph;

            var toolbar = new IMGUIContainer(() =>
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                if (GUILayout.Button("Save Asset", EditorStyles.toolbarButton))
                {
                    if (saveRequested != null)
                        saveRequested();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            });
            Add(toolbar);

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

            m_SearchWindowProvider = ScriptableObject.CreateInstance<SearchWindowProvider>();
            m_SearchWindowProvider.Initialize(editorWindow, m_Graph, m_GraphView);
            m_GraphView.nodeCreationRequest = (c) =>
            {
                SearchWindow.Open(new SearchWindowContext(c.screenMousePosition), m_SearchWindowProvider);
            };

            m_EdgeConnectorListener = new EdgeConnectorListener(m_Graph);

            foreach (var node in graph.GetNodes<INode>())
                AddNode(node);

            foreach (var edge in graph.edges)
                AddEdge(edge);

            Add(content);
        }

        GraphViewChange GraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.movedElements != null)
            {
                foreach (var element in graphViewChange.movedElements)
                {
                    var node = element.userData as INode;
                    if (node == null)
                        continue;

                    var drawState = node.drawState;
                    drawState.position = element.GetPosition();
                    node.drawState = drawState;
                }
            }

            return graphViewChange;
        }

        HashSet<NodeView> m_NodeViewHashSet = new HashSet<NodeView>();

        public void HandleGraphChanges()
        {
            foreach (var node in m_Graph.removedNodes)
            {
                var nodeView = m_GraphView.nodes.ToList().OfType<NodeView>().FirstOrDefault(p => p.node != null && p.node.guid == node.guid);
                if (nodeView != null)
                {
                    nodeView.Dispose();
                    nodeView.userData = null;
                    m_GraphView.RemoveElement(nodeView);
                }
            }

            foreach (var node in m_Graph.addedNodes)
            {
                AddNode(node);
            }
            
            var nodesToUpdate = m_NodeViewHashSet;
            nodesToUpdate.Clear();

            foreach (var edge in m_Graph.removedEdges)
            {
                var edgeView = m_GraphView.graphElements.ToList().OfType<EdgeView>().FirstOrDefault(p => p.userData is IEdge && Equals((IEdge)p.userData, edge));
                if (edgeView != null)
                {
                    var nodeView = edgeView.input.node as NodeView;
                    if (nodeView != null && nodeView.node != null)
                    {
                        nodesToUpdate.Add(nodeView);
                    }
                    edgeView.output.Disconnect(edgeView);
                    edgeView.input.Disconnect(edgeView);

                    edgeView.output = null;
                    edgeView.input = null;

                    m_GraphView.RemoveElement(edgeView);
                }
            }

            foreach (var edge in m_Graph.addedEdges)
            {
                var edgeView = AddEdge(edge);
                if (edgeView != null)
                    nodesToUpdate.Add((NodeView)edgeView.input.node);
            }

            foreach (var node in nodesToUpdate)
            {
            }
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

            if (m_SearchWindowProvider != null)
            {
                Object.DestroyImmediate(m_SearchWindowProvider);
                m_SearchWindowProvider = null;
            }
        }
    }
}
