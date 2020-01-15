using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
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