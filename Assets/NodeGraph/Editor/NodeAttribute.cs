﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewNodeAttribute : Attribute
    {
        public Type NodeType;

        public ViewNodeAttribute(Type type)
        {
            NodeType = type;
        }
    }
}