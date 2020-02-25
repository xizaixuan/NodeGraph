using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    [Serializable]
    public abstract class ModifierNode : INode, ISerializationCallbackReceiver
    {
        [NonSerialized]
        private Guid m_Guid;

        [SerializeField]
        private string m_GuidSerialized;

        [SerializeField]
        private string m_Name;

        [SerializeField]
        private DrawState m_DrawState;

        [NonSerialized]
        private List<ISlot> m_Slots = new List<ISlot>();

        [SerializeField]
        List<SerializationHelper.JSONSerializedElement> m_SerializableSlots = new List<SerializationHelper.JSONSerializedElement>();

        public IGraph owner { get; set; }

        public DrawState drawState
        {
            get { return m_DrawState; }
            set { m_DrawState = value; }
        }

        protected ModifierNode()
        {
            m_Guid = Guid.NewGuid();
        }

        public Guid guid
        {
            get { return m_Guid; }
        }

        public string name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public void GetInputSlots<T>(List<T> foundSlots) where T : ISlot
        {
            foreach (var slot in m_Slots)
            {
                if (slot.isInputSlot && slot is T)
                    foundSlots.Add((T)slot);
            }
        }

        public void GetOutputSlots<T>(List<T> foundSlots) where T : ISlot
        {
            foreach (var slot in m_Slots)
            {
                if (slot.isOutputSlot && slot is T)
                    foundSlots.Add((T)slot);
            }
        }

        public void GetSlots<T>(List<T> foundSlots) where T : ISlot
        {
            foreach (var slot in m_Slots)
            {
                if (slot is T)
                    foundSlots.Add((T)slot);
            }
        }

        public void AddSlot(ISlot slot)
        {
            if (!(slot is ModifierSlot))
                throw new ArgumentException(string.Format("Trying to add slot {0} to Modifier node {1}, but it is not a {2}", slot, this, typeof(ModifierSlot)));

            var addingSlot = (ModifierSlot)slot;
            var foundSlot = FindSlot<ModifierSlot>(slot.id);

            // this will remove the old slot and add a new one
            // if an old one was found. This allows updating values
            m_Slots.RemoveAll(x => x.id == slot.id);
            m_Slots.Add(slot);
            slot.owner = this;

            if (foundSlot == null)
                return;

            addingSlot.CopyValuesFrom(foundSlot);
        }

        public void RemoveSlot(int slotId)
        {
            //remove slots
            m_Slots.RemoveAll(x => x.id == slotId);
        }

        public T FindSlot<T>(int slotId) where T : ISlot
        {
            foreach (var slot in m_Slots)
            {
                if (slot.id == slotId && slot is T)
                    return (T)slot;
            }
            return default(T);
        }

        public T FindInputSlot<T>(int slotId) where T : ISlot
        {
            foreach (var slot in m_Slots)
            {
                if (slot.isInputSlot && slot.id == slotId && slot is T)
                    return (T)slot;
            }
            return default(T);
        }

        public T FindOutputSlot<T>(int slotId) where T : ISlot
        {
            foreach (var slot in m_Slots)
            {
                if (slot.isOutputSlot && slot.id == slotId && slot is T)
                    return (T)slot;
            }
            return default(T);
        }

        public void OnBeforeSerialize()
        {
            m_GuidSerialized = m_Guid.ToString();
            m_SerializableSlots = SerializationHelper.Serialize<ISlot>(m_Slots);
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(m_GuidSerialized))
                m_Guid = new Guid(m_GuidSerialized);
            else
                m_Guid = Guid.NewGuid();

            m_Slots = SerializationHelper.Deserialize<ISlot>(m_SerializableSlots, GraphUtil.GetLegacyTypeRemapping());
            m_SerializableSlots = null;
            foreach (var s in m_Slots)
                s.owner = this;

            UpdateNodeAfterDeserialization();
        }

        public virtual void UpdateNodeAfterDeserialization()
        { }
    }
}
