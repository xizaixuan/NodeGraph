using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace ModifierNodeGraph
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StringControlAttribute : Attribute, IControlAttribute
    {
        string m_Label;

        public StringControlAttribute(string label = null)
        {
            m_Label = label;
        }

        public VisualElement InstantiateControl(ModifierNode node, PropertyInfo propertyInfo)
        {
            return new StringControlView(m_Label, node, propertyInfo);
        }
    }

    public class StringControlView : VisualElement
    {
        ModifierNode m_Node;
        PropertyInfo m_PropertyInfo;

        public StringControlView(string label, ModifierNode node, PropertyInfo propertyInfo)
        {
            AddStyleSheetPath("Styles/Controls/StringControlView");
            m_Node = node;
            m_PropertyInfo = propertyInfo;
            //if (!propertyInfo.PropertyType.IsEnum)
            //    throw new ArgumentException("Property must be an enum.", "propertyInfo");
            Add(new Label(label ?? ObjectNames.NicifyVariableName(propertyInfo.Name)));
            var textField = new TextField();
            //enumField.OnValueChanged(OnValueChanged);
            Add(textField);
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
