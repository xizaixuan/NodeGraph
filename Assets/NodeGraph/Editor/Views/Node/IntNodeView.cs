using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

namespace ModifierNodeGraph
{
    [NodeView(typeof(IntNodeView))]
    public class IntNodeView : NodeView
    {
        public override void Enable()
        {
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