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

        public IntNode()
        {
            AddSlot(new ModifierSlot(0, "InputSlot", "ModifierOutput", SlotType.Input));
            AddSlot(new ModifierSlot(1, "OutputSlot", "ModifierOutput", SlotType.Output));
        }
    }
}
