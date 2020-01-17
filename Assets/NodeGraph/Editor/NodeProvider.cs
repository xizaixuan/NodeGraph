using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ModifierNodeGraph
{
    public static class NodeProvider
    {
        private static Dictionary<Type, Type> m_NodeView = new Dictionary<Type, Type>();
        private static Dictionary<Type, Type> m_ModifierNode = new Dictionary<Type, Type>();
        private static Dictionary<string, Type> m_NodeMenu = new Dictionary<string, Type>();

        static NodeProvider()
        {
            foreach (var type in GetAllTypesInCurrentDomain())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (type.IsSubclassOf(typeof(NodeView)))
                        AddNodeView(type);
                    else if (type.IsSubclassOf(typeof(ModifierNode)))
                        AddNode(type);
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

        static void AddNodeView(Type type)
        {
            if (type.GetCustomAttributes(typeof(BindNodeAttribute), false) is BindNodeAttribute[] attrs && attrs.Length > 0)
            {
                m_NodeView[attrs.First().NodeType] = type;
            }
        }

        static void AddNode(Type type)
        {
            if (type.GetCustomAttributes(typeof(NodeMenuItemAttribute), false) is NodeMenuItemAttribute[] attrs && attrs.Length > 0)
            {
                m_NodeMenu[attrs.First().ItemName] = type;
            }
        }

        public static Dictionary<string, Type> GetNodeMenuEntries()
        {
            return m_NodeMenu;
        }

        public static Type GetNodeViewTypeFromNodeType(Type nodeType)
        {
            m_NodeView.TryGetValue(nodeType, out Type nodeView);

            return nodeView;
        }
    }
}
