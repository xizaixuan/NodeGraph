using System;
using System.Globalization;
using System.Linq;
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
        string m_Value;

        public StringControlView(string label, ModifierNode node, PropertyInfo propertyInfo)
        {
            AddStyleSheetPath("Styles/Controls/StringControlView");
            m_Node = node;
            m_PropertyInfo = propertyInfo;

            label = label ?? ObjectNames.NicifyVariableName(propertyInfo.Name);
            if (!string.IsNullOrEmpty(label))
                Add(new Label(label));

            m_Value = GetValue();

            var textField = new TextField();
            textField.RegisterCallback<MouseDownEvent>(Repaint);
            textField.RegisterCallback<MouseMoveEvent>(Repaint);
            textField.OnValueChanged(evt =>
            {
                SetValue(evt.newValue);
                this.MarkDirtyRepaint();
            });
            textField.RegisterCallback<InputEvent>(evt =>
            {
                var value = GetValue();
                SetValue(value);
                this.MarkDirtyRepaint();
            });
            textField.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Escape)
                {
                    var value = GetValue();
                    SetValue(value);
                    evt.StopPropagation();
                }
                this.MarkDirtyRepaint();
            });
            Add(textField);
        }

        string GetValue()
        {
            var value = m_PropertyInfo.GetValue(m_Node, null);
            return (string)value;
        }

        void SetValue(string value)
        {
            m_PropertyInfo.SetValue(m_Node, value, null);
        }

        void Repaint<T>(MouseEventBase<T> evt) where T : MouseEventBase<T>, new()
        {
            evt.StopPropagation();
            this.MarkDirtyRepaint();
        }
    }
}
