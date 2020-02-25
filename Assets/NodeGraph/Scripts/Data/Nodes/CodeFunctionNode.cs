using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor.Graphing;

namespace ModifierNodeGraph
{
    public abstract class CodeFunctionNode : ModifierNode
    {
        protected CodeFunctionNode()
        {
            UpdateNodeAfterDeserialization();
        }

        protected enum Binding
        {
            None,
        }

        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
        protected class SlotAttribute : Attribute
        {
            public int slotId { get; private set; }
            public Binding binding { get; private set; }
            public Vector4? defaultValue { get; private set; }

            public SlotAttribute(int mSlotId, Binding mImplicitBinding)
            {
                slotId = mSlotId;
                binding = mImplicitBinding;
            }
        }

        protected abstract MethodInfo GetFunctionToConvert();

    
        public override void UpdateNodeAfterDeserialization()
        {
            var method = GetFunctionToConvert();

            if (method == null)
                throw new ArgumentException("Mapped method is null on node" + this);

            // validate no duplicates
            var slotAtributes = method.GetParameters().Select(GetSlotAttribute).ToList();
            if (slotAtributes.Any(x => x == null))
                throw new ArgumentException("Missing SlotAttribute on " + method.Name);

            if (slotAtributes.GroupBy(x => x.slotId).Any(x => x.Count() > 1))
                throw new ArgumentException("Duplicate SlotAttribute on " + method.Name);

            foreach (var par in method.GetParameters())
            {
                var attribute = GetSlotAttribute(par);
                var name = GraphUtil.ConvertCamelCase(par.Name, true);

                AddSlot(CreateBoundSlot(attribute.binding, attribute.slotId, name, par.IsOut ? SlotType.Output : SlotType.Input));
            }
        }

        private static ModifierSlot CreateBoundSlot(Binding attributeBinding, int slotId, string displayName, SlotType slotType)
        {
            switch (attributeBinding)
            {
                case Binding.None:
                    return new ModifierSlot(slotId, displayName, slotType);
                default:
                    throw new ArgumentOutOfRangeException("attributeBinding", attributeBinding, null);
            }
        }

        private static SlotAttribute GetSlotAttribute([NotNull] ParameterInfo info)
        {
            var attrs = info.GetCustomAttributes(typeof(SlotAttribute), false).OfType<SlotAttribute>().ToList();
            return attrs.FirstOrDefault();
        }
    }
}
