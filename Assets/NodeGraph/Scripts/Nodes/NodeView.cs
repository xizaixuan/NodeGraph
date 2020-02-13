﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

namespace ModifierNodeGraph
{
    [BindNode(typeof(ModifierNode))]
    public class NodeView : Node
    {
        public NodeGraphView Owner = null;
        public ModifierNode TargetNode = null;
        public ModifierNode node { get { return TargetNode; } }


        public void Initialize(NodeGraphView owner, ModifierNode node)
        {
            Owner = owner;

            TargetNode = node;

            InitializePorts();

            InitializeView();

            Enable();
        }

        public void InitializePorts()
        {
            foreach (var slot in TargetNode.GetSlots<ModifierSlot>())
            {
                var port = ModifierPort.Create(slot, Owner.ConnectorListener);
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
    }
}