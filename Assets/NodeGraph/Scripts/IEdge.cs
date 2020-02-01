using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    public interface IEdge : IEquatable<IEdge>
    {
        SlotReference outputSlot { get; }
        SlotReference inputSlot { get; }
    }
}