using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace ModifierNodeGraph
{
    [NodeView(typeof(NodeView))]
    public class NodeView : Node
    {
        public ModifierNode TargetNode = null;

        public void Initialize(ModifierNode node)
        {
            TargetNode = node;

            Enable();
        }

        public virtual void Enable()
        {
        }
    }
}