using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModifierNodeGraph
{
    public static class NodeProvider
    {
        private static Dictionary<Type, Type> m_NodeViewType = new Dictionary<Type, Type>();

        static NodeProvider()
        {
            foreach (var type in GetAllTypesInCurrentDomain())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (type.IsSubclassOf(typeof(NodeView)))
                        AddViewNode(type);
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
                m_NodeViewType[attrs.First().NodeType] = type;
            }
        }
    }
}
