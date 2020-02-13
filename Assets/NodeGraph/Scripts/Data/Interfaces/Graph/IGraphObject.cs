using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    public interface IGraphObject
    {
        IGraph graph { get; set; }
    }
}

