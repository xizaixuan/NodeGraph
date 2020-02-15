using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace ModifierNodeGraph
{
    public class CreateModifierGraph : EndNameEditAction
    {
        [MenuItem("Assets/Create/ModifierGraph", false, 208)]
        public static void CreateGraph()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateModifierGraph>(),
                string.Format("New Modifier Graph.{0}", ModifierGraphImporter.Extension), null, null);
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var graph = new NodeGraph();
            File.WriteAllText(pathName, EditorJsonUtility.ToJson(graph));
            AssetDatabase.Refresh();
        }
    }
}
