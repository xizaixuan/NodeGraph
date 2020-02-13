using System;

namespace ModifierNodeGraph
{
    public interface ISlot : IEquatable<ISlot>
    {
        int id { get; }
        string displayName { get; set; }
        bool isInputSlot { get; }
        bool isOutputSlot { get; }
        SlotReference slotReference { get; }
        INode owner { get; set; }
    }
}
