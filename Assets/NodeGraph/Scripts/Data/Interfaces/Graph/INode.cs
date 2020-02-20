using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModifierNodeGraph
{
    public interface INode
    {
        IGraph owner { get; set; }
        Guid guid { get; }
        DrawState drawState { get; set; }
        string name { get; set; }
        void GetInputSlots<T>(List<T> foundSlots) where T : ISlot;
        void GetOutputSlots<T>(List<T> foundSlots) where T : ISlot;
        void GetSlots<T>(List<T> foundSlots) where T : ISlot;
        void AddSlot(ISlot slot);
        void RemoveSlot(int slotId);
        T FindSlot<T>(int slotId) where T : ISlot;
        T FindInputSlot<T>(int slotId) where T : ISlot;
        T FindOutputSlot<T>(int slotId) where T : ISlot;
    }

    public static class NodeExtensions
    {
        public static IEnumerable<T> GetSlots<T>(this INode node) where T : ISlot
        {
            var slots = new List<T>();
            node.GetSlots(slots);
            return slots;
        }

        public static IEnumerable<T> GetInputSlots<T>(this INode node) where T : ISlot
        {
            var slots = new List<T>();
            node.GetInputSlots(slots);
            return slots;
        }

        public static IEnumerable<T> GetOutputSlots<T>(this INode node) where T : ISlot
        {
            var slots = new List<T>();
            node.GetOutputSlots(slots);
            return slots;
        }
    }
}