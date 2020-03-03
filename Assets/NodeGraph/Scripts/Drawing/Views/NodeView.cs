using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

namespace ModifierNodeGraph
{
    public sealed class NodeView : Node
    {
        IEdgeConnectorListener m_ConnectorListener;
        VisualElement m_ControlItems;

        public ModifierNode node { get; private set; }

        public void Initialize(ModifierNode inNode, IEdgeConnectorListener connectorListener)
        {
            m_ConnectorListener = connectorListener;

            node = inNode;

            persistenceKey = node.guid.ToString();

            title = inNode.name;

            var contents = this.Q("contents");

            // Add controls container
            var controlsContainer = new VisualElement { name = "controls" };
            {
                m_ControlItems = new VisualElement { name = "items" };
                controlsContainer.Add(m_ControlItems);

                // Instantiate control views from node
                foreach (var propertyInfo in node.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    foreach (IControlAttribute attribute in propertyInfo.GetCustomAttributes(typeof(IControlAttribute), false))
                        m_ControlItems.Add(attribute.InstantiateControl(node, propertyInfo));
            }

            if (m_ControlItems.childCount > 0)
                contents.Add(controlsContainer);

            AddSlots(node.GetSlots<ModifierSlot>());

            SetPosition(new Rect(node.drawState.position.x, node.drawState.position.y, 0, 0));
        }

        void AddSlots(IEnumerable<ModifierSlot> slots)
        {
            foreach (var slot in slots)
            {
                var port = ModifierPort.Create(slot, m_ConnectorListener);
                if (slot.isOutputSlot)
                    outputContainer.Add(port);
                else
                    inputContainer.Add(port);
            }
        }

        public void Dispose()
        {
            node = null;
        }
    }
}