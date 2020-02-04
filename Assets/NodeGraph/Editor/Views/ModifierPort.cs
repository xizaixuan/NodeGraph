using System;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

using EdgeView = UnityEditor.Experimental.UIElements.GraphView.Edge;

namespace ModifierNodeGraph
{
    public class ModifierPort : Port
    {
        ModifierPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type)
                    : base(portOrientation, portDirection, portCapacity, type)
        {
        }

        ModifierSlot m_Slot;

        public static Port Create(ModifierSlot slot, IEdgeConnectorListener connectorListener)
        {
            var port = new ModifierPort(Orientation.Horizontal, slot.isInputSlot ? Direction.Input : Direction.Output,
                    slot.isInputSlot ? Capacity.Single : Capacity.Multi, null)
            {
                m_EdgeConnector = new EdgeConnector<EdgeView>(connectorListener),
            };
            port.AddManipulator(port.m_EdgeConnector);
            port.slot = slot;
            port.portName = slot.displayName;
            return port;
        }

        public ModifierSlot slot
        {
            get { return m_Slot; }
            set
            {
                if (ReferenceEquals(value, m_Slot))
                    return;
                if (value == null)
                    throw new NullReferenceException();
                if (m_Slot != null && value.isInputSlot != m_Slot.isInputSlot)
                    throw new ArgumentException("Cannot change direction of already created port");
                m_Slot = value;
                portName = slot.displayName;
            }
        }
    }
}
