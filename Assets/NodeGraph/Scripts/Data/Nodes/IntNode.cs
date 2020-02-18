using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    [Title("Input")]
    public class IntNode : ModifierNode
    {
        public IntNode()
        {
            AddSlot(new ModifierSlot(0, "InputSlot", "ModifierOutput", SlotType.Input));
            AddSlot(new ModifierSlot(1, "OutputSlot", "ModifierOutput", SlotType.Output));
        }
    }
}
