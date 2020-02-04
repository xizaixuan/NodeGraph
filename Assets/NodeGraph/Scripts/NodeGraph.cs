using ModifierNodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "NodeGraph", menuName = "NodeGraph")]
public class NodeGraph : ScriptableObject, IGraph
{
    public List<INode> Nodes = new List<INode>();
    public List<IEdge> Edges = new List<IEdge>();

    public void AddNode(INode node)
    {
        Nodes.Add(node);
    }

    public void RemoveNode(INode node)
    {
        Nodes.Remove(node);
    }
}
