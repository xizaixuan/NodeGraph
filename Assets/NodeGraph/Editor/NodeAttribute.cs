using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeViewAttribute : Attribute
    {
        public Type NodeType;

        public NodeViewAttribute(Type type)
        {
            NodeType = type;
        }
    }
}