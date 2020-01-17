using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace ModifierNodeGraph
{
    public class PortView : Port
    {
        public PortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type, IEdgeConnectorListener connectorListener, string name)
            : base(portOrientation, portDirection, portCapacity, type)
        {
            portName = name;
            m_EdgeConnector = new EdgeConnector<Edge>(connectorListener);

            this.AddManipulator(m_EdgeConnector);
        }
    }
}
