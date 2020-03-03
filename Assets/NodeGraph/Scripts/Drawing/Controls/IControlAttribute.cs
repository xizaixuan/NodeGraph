using System.Reflection;
using UnityEngine.Experimental.UIElements;

namespace ModifierNodeGraph
{
    public interface IControlAttribute
    {
        VisualElement InstantiateControl(ModifierNode node, PropertyInfo propertyInfo);
    }
}
