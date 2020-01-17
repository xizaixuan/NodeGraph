using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    [NodeMenuItem("CreateNode/IntNode")]
    public class IntNode : ModifierNode
    {
        [Input]
        public int Value;
    }
}
