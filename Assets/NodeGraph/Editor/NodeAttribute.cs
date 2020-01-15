using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNode
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InputAttribute : Attribute
    {
        public string Name;

        public InputAttribute(string name = null)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class OutputAttribute : Attribute
    {
        public string Name;

        public OutputAttribute(string name = null)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class NodeViewAttribute : Attribute
    {
        public Type NodeType;

        public NodeViewAttribute()
        {
            NodeType = GetType();
        }
    }
}