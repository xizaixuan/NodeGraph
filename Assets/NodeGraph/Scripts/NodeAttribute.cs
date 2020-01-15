using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
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
    public class ModifierNodeAttribute : Attribute
    {
        public Type NodeType;

        public ModifierNodeAttribute(Type type)
        {
            NodeType = type;
        }
    }
}