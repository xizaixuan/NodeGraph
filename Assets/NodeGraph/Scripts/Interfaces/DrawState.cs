using System;
using UnityEngine;

namespace ModifierNodeGraph
{
    [Serializable]
    public struct DrawState
    {
        private Rect m_Position;

        public Rect position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }
    }
}
