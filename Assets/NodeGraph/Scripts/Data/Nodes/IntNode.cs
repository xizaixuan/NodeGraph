using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ModifierNodeGraph
{
    [Title("Input")]
    public class IntNode : CodeFunctionNode
    {
        public IntNode()
        {
            name = "Input";
        }

        protected override MethodInfo GetFunctionToConvert()
        {
            return GetType().GetMethod("Function", BindingFlags.Static | BindingFlags.NonPublic);
        }

        static void Function(
            [Slot(0, Binding.None)] Vector3 In111,
            [Slot(1, Binding.None)] out Vector3 Out222)
        {
            Out222 = Vector3.zero;
        }
    }
}
