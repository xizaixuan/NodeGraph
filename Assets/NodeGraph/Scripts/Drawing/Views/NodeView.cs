using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

namespace ModifierNodeGraph
{
    public class NodeView : Node
    {
        IEdgeConnectorListener m_ConnectorListener;

        public ModifierNode TargetNode = null;
        public ModifierNode node { get { return TargetNode; } }



        public void Initialize(ModifierNode node, IEdgeConnectorListener connectorListener)
        {
            m_ConnectorListener = connectorListener;

            TargetNode = node;

            InitializePorts();

            InitializeView();

            Enable();
        }

        public void InitializePorts()
        {
            foreach (var slot in TargetNode.GetSlots<ModifierSlot>())
            {
                var port = ModifierPort.Create(slot, m_ConnectorListener);
                if (slot.isOutputSlot)
                    outputContainer.Add(port);
                else
                    inputContainer.Add(port);
            }
        }

        void InitializeView()
        {
            SetPosition(TargetNode.drawState.position);
        }

        public virtual void Enable()
        {
        }

        public override void SetPosition(Rect newPosition)
        {
            base.SetPosition(newPosition);

            var drawState = TargetNode.drawState;
            drawState.position = newPosition;
            TargetNode.drawState = drawState;
        }

        public void Dispose()
        {
        }
    }
}