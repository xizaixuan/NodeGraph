using ModifierNodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "NodeGraph", menuName = "NodeGraph")]
public class NodeGraph : ScriptableObject, IGraph
{
    public List<IEdge> Edges = new List<IEdge>();

    List<ModifierNode> m_Nodes = new List<ModifierNode>();

    Dictionary<Guid, INode> m_NodeDictionary = new Dictionary<Guid, INode>();

    public IEnumerable<T> GetNodes<T>() where T : INode
    {
        return m_Nodes.Where(x => x != null).OfType<T>();
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
    }

    public INode GetNodeFromGuid(Guid guid)
    {
        INode node;
        m_NodeDictionary.TryGetValue(guid, out node);
        return node;
    }
}
