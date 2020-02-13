using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    public class GraphObject : ScriptableObject, IGraphObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        SerializationHelper.JSONSerializedElement m_SerializedGraph;

        IGraph m_Graph;
        IGraph m_DeserializedGraph;

        public IGraph graph
        {
            get { return m_Graph; }
            set
            {
                if (m_Graph != null)
                    m_Graph.owner = null;
                m_Graph = value;
                if (m_Graph != null)
                    m_Graph.owner = this;
            }
        }

        public void OnBeforeSerialize()
        {
            if (graph != null)
                m_SerializedGraph = SerializationHelper.Serialize(graph);
        }

        public void OnAfterDeserialize()
        {
            var deserializedGraph = SerializationHelper.Deserialize<IGraph>(m_SerializedGraph, null);
            if (graph == null)
                graph = deserializedGraph;
            else
                m_DeserializedGraph = deserializedGraph;
        }
    }
}