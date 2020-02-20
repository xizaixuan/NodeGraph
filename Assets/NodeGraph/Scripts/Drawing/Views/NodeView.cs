using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

namespace ModifierNodeGraph
{
    public sealed class NodeView : Node
    {
        IEdgeConnectorListener m_ConnectorListener;

        public ModifierNode node { get; private set; }

        public void Initialize(ModifierNode inNode, IEdgeConnectorListener connectorListener)
        {
            m_ConnectorListener = connectorListener;

            node = inNode;

            persistenceKey = node.guid.ToString();

            title = inNode.name;

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