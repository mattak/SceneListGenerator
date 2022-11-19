using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SceneListGenerator.EditorRuntime
{
    [CreateAssetMenu(fileName = "SceneListGeneratorSetting", menuName = "SceneListGeneratorSetting")]
    [Serializable]
    public class SceneListGeneratorSetting : ScriptableObject
    {
        public bool IsEnableAssetPostprocessor;
        public string Namespace;
        public string WritePath;

        private void Reset()
        {
            this.IsEnableAssetPostprocessor = true;
            this.Namespace = "DefaultNamespace";
            this.WritePath = "Assets/Scripts/SceneListGenerated.cs";
        }

        public static SceneListGeneratorSetting LoadFromAsset()
        {
            var scriptableObject = AssetDatabase.FindAssets("t:ScriptableObject", new[] {"Assets"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(path => AssetDatabase.LoadAssetAtPath<SceneListGeneratorSetting>(path))
                .FirstOrDefault(x => x != null);
            if (scriptableObject != null) return scriptableObject;
            return null;
        }
    }
}