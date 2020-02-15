using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor;

namespace ModifierNodeGraph
{
    [ScriptedImporter(20, Extension)]
    public class ModifierGraphImporter : ScriptedImporter
    {
        public const string Extension = "modifiergraph";

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var oldShader = AssetDatabase.LoadAssetAtPath<Shader>(ctx.assetPath);
            if (oldShader != null)
                ShaderUtil.ClearShaderErrors(oldShader);

            string path = ctx.assetPath;
            var sourceAssetDependencyPaths = new List<string>();

            foreach (var sourceAssetDependencyPath in sourceAssetDependencyPaths.Distinct())
                ctx.DependsOnSourceAsset(sourceAssetDependencyPath);
        }
    }
}
