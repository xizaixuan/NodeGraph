using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModifierNodeGraph
{
    public static class NodeProvider
    {
        private static Dictionary<Type, Type> m_ViewNodeType = new Dictionary<Type, Type>();
        private static Dictionary<Type, Type> m_ModifierNodeType = new Dictionary<Type, Type>();

        static NodeProvider()
        {
            foreach (var type in GetAllTypesInCurrentDomain())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (type.IsSubclassOf(typeof(NodeView)))
                        AddViewNode(type);
                    else if (type.IsSubclassOf(typeof(ModifierNode)))
                        AddModifierNode(type);
                }
            }
        }

        static IEnumerable<Type> GetAllTypesInCurrentDomain()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = { };

                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    //just ignore it ...
                }

                foreach (var type in types)
                    yield return type;
            }
        }

        static void AddViewNode(Type type)
        {
            if (type.GetCustomAttributes(typeof(ViewNodeAttribute), false) is ViewNodeAttribute[] attrs && attrs.Length > 0)
            {
                m_ViewNodeType[attrs.First().NodeType] = type;
            }
        }

        static void AddModifierNode(Type type)
        {
            if (type.GetCustomAttributes(typeof(ModifierNodeAttribute), false) is ModifierNodeAttribute[] attrs && attrs.Length > 0)
            {
                m_ModifierNodeType[attrs.First().NodeType] = type;
            }
        }
    }
}
