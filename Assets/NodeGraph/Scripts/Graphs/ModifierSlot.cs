using System;

namespace ModifierNodeGraph
{
    public class ModifierSlot : ISlot
    {
        int m_Id;

        string m_DisplayName = "Not Initilaized";

        string m_ModifierOutputName;

        public ModifierSlot(int slotId, string displayName, string modifierOutputName, SlotType slotType)
        {
            m_Id = slotId;
            m_DisplayName = displayName;
            m_SlotType = slotType;
            m_ModifierOutputName = modifierOutputName;
        }

        public int id
        {
            get { return m_Id; }
        }

        public virtual string displayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; }
        }

        SlotType m_SlotType = SlotType.Input;

        public bool isInputSlot
        {
            get { return m_SlotType == SlotType.Input; }
        }

        public bool isOutputSlot
        {
            get { return m_SlotType == SlotType.Output; }
        }

        public SlotReference slotReference
        {
            get { return new SlotReference(owner.guid, m_Id); }
        }

        public INode owner { get; set; }

        public string modifierOutputName
        {
            get { return m_ModifierOutputName; }
            private set { m_ModifierOutputName = value; }
        }

        public bool IsCompatibleWith(ModifierSlot otherSlot)
        {
            return otherSlot != null
                && otherSlot.owner != owner
                && otherSlot.isInputSlot != isInputSlot;
        }

        public virtual void CopyValuesFrom(ModifierSlot foundSlot)
        {
        }

        public bool Equals(ModifierSlot other)
        {
            return m_Id == other.m_Id && owner.guid.Equals(other.owner.guid);
        }

        public bool Equals(ISlot other)
        {
            return Equals(other as object);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ModifierSlot)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (m_Id * 397) ^ (owner != null ? owner.GetHashCode() : 0);
            }
        }
    }
}