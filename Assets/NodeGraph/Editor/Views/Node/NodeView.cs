using System.Collections;
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
            var inputPort = new PortView(Orientation.Horizontal, Direction.Input, Capacity.Single, typeof(int), Owner.ConnectorListener, "Input");
            var outputPort = new PortView(Orientation.Horizontal, Direction.Output, Capacity.Single, typeof(int), Owner.ConnectorListener, "Output");

            this.inputContainer.Add(inputPort);
            this.outputContainer.Add(outputPort);
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