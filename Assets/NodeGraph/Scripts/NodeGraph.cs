using ModifierNodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "NodeGraph", menuName = "NodeGraph")]
public class NodeGraph : ScriptableObject
{
    public List<ModifierNode> Nodes = new List<ModifierNode>();

    public void AddNode(ModifierNode node)
    {
        Nodes.Add(node);
    }
}
