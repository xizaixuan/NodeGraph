using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEditor.Graphing;
using UnityEditor.Graphing.Util;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace ModifierNodeGraph
{
    class SearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        EditorWindow m_EditorWindow;
        NodeGraph m_Graph;
        GraphView m_GraphView;

        public void Initialize(EditorWindow editorWindow, NodeGraph graph, GraphView graphView)
        {
            m_EditorWindow = editorWindow;
            m_Graph = graph;
            m_GraphView = graphView;
        }

        struct NodeEntry
        {
            public string[] title;
            public ModifierNode node;

        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            // First build up temporary data structure containing group & title as an array of strings (the last one is the actual title) and associated node type.
            var nodeEntries = new List<NodeEntry>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypesOrNothing())
                {
                    if (type.IsClass && !type.IsAbstract && (type.IsSubclassOf(typeof(ModifierNode))))
                    {
                        var attrs = type.GetCustomAttributes(typeof(TitleAttribute), false) as TitleAttribute[];
                        if (attrs != null && attrs.Length > 0)
                        {
                            var node = (ModifierNode)Activator.CreateInstance(type);
                            AddEntries(node, attrs[0].title, nodeEntries);
                        }
                    }
                }
            }

            nodeEntries.Sort((entry1, entry2) =>
            {
                for (var i = 0; i < entry1.title.Length; i++)
                {
                    if (i >= entry2.title.Length)
                        return 1;
                    var value = entry1.title[i].CompareTo(entry2.title[i]);
                    if (value != 0)
                    {
                        // Make sure that leaves go before nodes
                        if (entry1.title.Length != entry2.title.Length && (i == entry1.title.Length - 1 || i == entry2.title.Length - 1))
                            return entry1.title.Length < entry2.title.Length ? -1 : 1;
                        return value;
                    }
                }
                return 0;
            });
            
            var groups = new List<string>();

            // First item in the tree is the title of the window.
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
            };

            foreach (var nodeEntry in nodeEntries)
            {
                // `createIndex` represents from where we should add new group entries from the current entry's group path.
                var createIndex = int.MaxValue;

                // Compare the group path of the current entry to the current group path.
                for (var i = 0; i < nodeEntry.title.Length - 1; i++)
                {
                    var group = nodeEntry.title[i];
                    if (i >= groups.Count)
                    {
                        // The current group path matches a prefix of the current entry's group path, so we add the
                        // rest of the group path from the currrent entry.
                        createIndex = i;
                        break;
                    }
                    if (groups[i] != group)
                    {
                        // A prefix of the current group path matches a prefix of the current entry's group path,
                        // so we remove everyfrom from the point where it doesn't match anymore, and then add the rest
                        // of the group path from the current entry.
                        groups.RemoveRange(i, groups.Count - i);
                        createIndex = i;
                        break;
                    }
                }

                // Create new group entries as needed.
                // If we don't need to modify the group path, `createIndex` will be `int.MaxValue` and thus the loop won't run.
                for (var i = createIndex; i < nodeEntry.title.Length - 1; i++)
                {
                    var group = nodeEntry.title[i];
                    groups.Add(group);
                    tree.Add(new SearchTreeGroupEntry(new GUIContent(group)) { level = i + 1 });
                }

                // Finally, add the actual entry.
                tree.Add(new SearchTreeEntry(new GUIContent(nodeEntry.title.Last(), "")) { level = nodeEntry.title.Length, userData = nodeEntry });
            }
            
            return tree;
        }


        void AddEntries(ModifierNode node, string[] title, List<NodeEntry> nodeEntries)
        {
            nodeEntries.Add(new NodeEntry
            {
                node = node,
                title = title,
            });
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            var nodeEntry = (NodeEntry)entry.userData;
            var node = nodeEntry.node;

            var drawState = node.drawState;
            var windowMousePosition = m_EditorWindow.GetRootVisualContainer().ChangeCoordinatesTo(m_EditorWindow.GetRootVisualContainer().parent, context.screenMousePosition - m_EditorWindow.position.position);
            var graphMousePosition = m_GraphView.contentViewContainer.WorldToLocal(windowMousePosition);
            drawState.position = new Rect(graphMousePosition, Vector2.zero);
            node.drawState = drawState;
            
            m_Graph.AddNode(node);

            return true;
        }
    }
}
