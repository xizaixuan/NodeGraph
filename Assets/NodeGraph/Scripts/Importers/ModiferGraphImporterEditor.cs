using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace ModifierNodeGraph
{

    [CustomEditor(typeof(ModifierGraphImporter))]
    public class ModifierGraphImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Modifier Editor"))
            {
                AssetImporter importer = target as AssetImporter;
                Debug.Assert(importer != null, "importer != null");
                ShowGraphEditWindow(importer.assetPath);
            }
        }

        internal static bool ShowGraphEditWindow(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            var extension = Path.GetExtension(path);
            if (extension == null)
                return false;
            // Path.GetExtension returns the extension prefixed with ".", so we remove it. We force lower case such that
            // the comparison will be case-insensitive.
            extension = extension.Substring(1).ToLowerInvariant();
            if (extension != ModifierGraphImporter.Extension)
                return false;

            var foundWindow = false;
            foreach (var w in Resources.FindObjectsOfTypeAll<NodeGraphWindow>())
            {
                if (w.selectedGuid == guid)
                {
                    foundWindow = true;
                    w.Focus();
                }
            }

            if (!foundWindow)
            {
                var window = CreateInstance<NodeGraphWindow>();
                window.Show();
                window.Initialize(guid);
            }

            return true;
        }

        [OnOpenAsset(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var path = AssetDatabase.GetAssetPath(instanceID);
            return ShowGraphEditWindow(path);
        }
    }
}
