using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    [ModifierNode(typeof(IntNode))]
    public class IntNode : ModifierNode
    {
        [Input]
        public int Value;
    }
}
