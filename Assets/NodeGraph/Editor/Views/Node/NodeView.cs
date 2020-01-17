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
        public ModifierNode TargetNode = null;

        public void Initialize(ModifierNode node)
        {
            TargetNode = node;

            InitializePorts();

            Enable();
        }

        public void InitializePorts()
        {
            var inputPort = new PortView(Orientation.Horizontal, Direction.Input, Capacity.Single, typeof(int), "Input");
            var outputPort = new PortView(Orientation.Horizontal, Direction.Output, Capacity.Single, typeof(int), "Output");

            this.inputContainer.Add(inputPort);
            this.outputContainer.Add(outputPort);
        }

        public virtual void Enable()
        {
        }
    }
}