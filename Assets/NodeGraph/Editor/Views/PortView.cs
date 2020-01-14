using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class PortView : Port
{
    public PortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type, string name)
        : base(portOrientation, portDirection, portCapacity, type)
    {
        portName = name;
    }
}
