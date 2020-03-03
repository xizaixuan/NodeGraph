using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace ModifierNodeGraph
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EnumControlAttribute : Attribute, IControlAttribute
    {
        string m_Label;

        public EnumControlAttribute(string label = null)
        {
            m_Label = label;
        }

        public VisualElement InstantiateControl(ModifierNode node, PropertyInfo propertyInfo)
        {
            return new EnumControlView(m_Label, node, propertyInfo);
        }
    }

    public class EnumControlView : VisualElement
    {
        ModifierNode m_Node;
        PropertyInfo m_PropertyInfo;

        public EnumControlView(string label, ModifierNode node, PropertyInfo propertyInfo)
        {
            AddStyleSheetPath("Styles/Controls/EnumControlView");
            m_Node = node;
            m_PropertyInfo = propertyInfo;
            if (!propertyInfo.PropertyType.IsEnum)
                throw new ArgumentException("Property must be an enum.", "propertyInfo");
            Add(new Label(label ?? ObjectNames.NicifyVariableName(propertyInfo.Name)));
            var enumField = new EnumField((Enum)m_PropertyInfo.GetValue(m_Node, null));
            enumField.OnValueChanged(OnValueChanged);
            Add(enumField);
        }

        void OnValueChanged(ChangeEvent<Enum> evt)
        {
            var value = (Enum)m_PropertyInfo.GetValue(m_Node, null);
            if (!evt.newValue.Equals(value))
            {
                m_PropertyInfo.SetValue(m_Node, evt.newValue, null);
            }
        }
    }
}
