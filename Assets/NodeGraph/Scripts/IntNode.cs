using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    [Node(typeof(IntNode))]
    public class IntNode : ModifierNode
    {
        [Input]
        public int Value;
    }
}
