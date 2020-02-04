using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    public abstract class ModifierNode : INode
    {
        private DrawState m_DrawState;

        public DrawState drawState
        {
            get { return m_DrawState; }
            set { m_DrawState = value; }
        }
    }
}
