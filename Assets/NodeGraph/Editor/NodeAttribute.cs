using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BindNodeAttribute : Attribute
    {
        public Type NodeType;

        public BindNodeAttribute(Type type)
        {
            NodeType = type;
        }
    }
}