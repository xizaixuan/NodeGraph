using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using static UnityEditor.Experimental.UIElements.GraphView.Port;

namespace ModifierNodeGraph
{
    [BindNode(typeof(IntNode))]
    public class IntNodeView : NodeView
    {
        public override void Enable()
        {
            title = "IntNode";

            var intNode = TargetNode as IntNode;

            IntegerField intField = new IntegerField
            {
                value = intNode.Value
            };

            intField.OnValueChanged((v) =>
            {
                intNode.Value = (int)v.newValue;
            });

            mainContainer.Add(intField);
        }
    }
}