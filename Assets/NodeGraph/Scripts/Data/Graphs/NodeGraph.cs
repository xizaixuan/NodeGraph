﻿using ModifierNodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModifierNodeGraph
{
    [Serializable]
    public class NodeGraph : IGraph, ISerializationCallbackReceiver
    {
        public IGraphObject owner { get; set; }

        #region Node data

        List<ModifierNode> m_Nodes = new List<ModifierNode>();

        Dictionary<Guid, INode> m_NodeDictionary = new Dictionary<Guid, INode>();

        public IEnumerable<T> GetNodes<T>() where T : INode
        {
            return m_Nodes.Where(x => x != null).OfType<T>();
        }

        [SerializeField]
        List<SerializationHelper.JSONSerializedElement> m_SerializableNodes = new List<SerializationHelper.JSONSerializedElement>();

        [NonSerialized]
        List<INode> m_AddedNodes = new List<INode>();

        public IEnumerable<INode> addedNodes
        {
            get { return m_AddedNodes; }
        }

        [NonSerialized]
        List<INode> m_RemovedNodes = new List<INode>();

        public IEnumerable<INode> removedNodes
        {
            get { return m_RemovedNodes; }
        }

        #endregion

        #region Edge data
        List<IEdge> m_Edges = new List<IEdge>();

        public IEnumerable<IEdge> edges
        {
            get { return m_Edges; }
        }

        [SerializeField]
        List<SerializationHelper.JSONSerializedElement> m_SerializableEdges = new List<SerializationHelper.JSONSerializedElement>();

        [NonSerialized]
        List<IEdge> m_AddedEdges = new List<IEdge>();

        public IEnumerable<IEdge> addedEdges
        {
            get { return m_AddedEdges; }
        }

        [NonSerialized]
        List<IEdge> m_RemovedEdges = new List<IEdge>();

        public IEnumerable<IEdge> removedEdges
        {
            get { return m_RemovedEdges; }
        }

        #endregion

        public void ClearChanges()
        {
            m_AddedNodes.Clear();
            m_RemovedNodes.Clear();
            m_AddedEdges.Clear();
            m_RemovedEdges.Clear();
        }

        public void AddNode(INode node)
        {
            if (node is ModifierNode)
            {
                AddNodeNoValidate(node);
            }
            else
            {
                Debug.LogWarningFormat("Trying to add node {0} to Node graph, but it is not a {1}", node, typeof(ModifierNode));
            }
        }

        void AddNodeNoValidate(INode node)
        {
            var modifierNode = (ModifierNode)node;
            modifierNode.owner = this;

            m_Nodes.Add(modifierNode);
            m_NodeDictionary.Add(modifierNode.guid, modifierNode);
            m_AddedNodes.Add(modifierNode);
        }

        public void RemoveNode(INode node)
        {
            RemoveNodeNoValidate(node);
        }

        void RemoveNodeNoValidate(INode node)
        {
            var modifierNode = (ModifierNode)node;

            m_Nodes.Remove(node as ModifierNode);
            m_NodeDictionary.Remove(modifierNode.guid);
            m_RemovedNodes.Add(node);
        }

        IEdge ConnectNoValidate(SlotReference fromSlotRef, SlotReference toSlotRef)
        {
            var fromNode = GetNodeFromGuid(fromSlotRef.nodeGuid);
            var toNode = GetNodeFromGuid(toSlotRef.nodeGuid);

            if (fromNode == null || toNode == null)
                return null;

            //         // if fromNode is already connected to toNode
            //         // do now allow a connection as toNode will then
            //         // have an edge to fromNode creating a cycle.
            //         // if this is parsed it will lead to an infinite loop.
            //         var dependentNodes = new List<INode>();
            //         NodeUtils.CollectNodesNodeFeedsInto(dependentNodes, toNode);
            //         if (dependentNodes.Contains(fromNode))
            //             return null;

            var fromSlot = fromNode.FindSlot<ISlot>(fromSlotRef.slotId);
            var toSlot = toNode.FindSlot<ISlot>(toSlotRef.slotId);

            if (fromSlot.isOutputSlot == toSlot.isOutputSlot)
                return null;

            var outputSlot = fromSlot.isOutputSlot ? fromSlotRef : toSlotRef;
            var inputSlot = fromSlot.isInputSlot ? fromSlotRef : toSlotRef;

            //         s_TempEdges.Clear();
            //         GetEdges(inputSlot, s_TempEdges);

            //         // remove any inputs that exits before adding
            //         foreach (var edge in s_TempEdges)
            //         {
            //             RemoveEdgeNoValidate(edge);
            //         }

            var newEdge = new Edge(outputSlot, inputSlot);
            m_Edges.Add(newEdge);
            m_AddedEdges.Add(newEdge);
            //        AddEdgeToNodeEdges(newEdge);

            //Debug.LogFormat("Connected edge: {0} -> {1} ({2} -> {3})\n{4}", newEdge.outputSlot.nodeGuid, newEdge.inputSlot.nodeGuid, fromNode.name, toNode.name, Environment.StackTrace);
            return newEdge;
        }

        public IEdge Connect(SlotReference fromSlotRef, SlotReference toSlotRef)
        {
            var newEdge = ConnectNoValidate(fromSlotRef, toSlotRef);
            return newEdge;
        }

        public INode GetNodeFromGuid(Guid guid)
        {
            INode node;
            m_NodeDictionary.TryGetValue(guid, out node);
            return node;
        }

        public void OnBeforeSerialize()
        {
            m_SerializableNodes = SerializationHelper.Serialize(GetNodes<INode>());
            m_SerializableEdges = SerializationHelper.Serialize<IEdge>(m_Edges);
        }

        public void OnAfterDeserialize()
        {
            var nodes = SerializationHelper.Deserialize<INode>(m_SerializableNodes, GraphUtil.GetLegacyTypeRemapping());
            m_Nodes = new List<ModifierNode>(nodes.Count);
            m_NodeDictionary = new Dictionary<Guid, INode>(nodes.Count);
            foreach (var node in nodes.OfType<ModifierNode>())
            {
                node.owner = this;
                node.UpdateNodeAfterDeserialization();
                m_Nodes.Add(node);
                m_NodeDictionary.Add(node.guid, node);
            }
            m_SerializableNodes = null;

            m_Edges = SerializationHelper.Deserialize<IEdge>(m_SerializableEdges, GraphUtil.GetLegacyTypeRemapping());
            m_SerializableEdges = null;
        }
    }
}
