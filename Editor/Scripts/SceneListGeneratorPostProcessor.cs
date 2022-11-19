using UnityEditor;

namespace SceneListGenerator.EditorRuntime
{
    public class SceneListGeneratorPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            if (importedAssets.Length > 0)
            {
                var path = importedAssets[0];
                if (path.EndsWith(".unity") && path.StartsWith("Assets"))
                {
                    var unityScenePath = importedAssets[0];
                    Update(unityScenePath);
                }
            }
        }

        private static void Update(string unityScenePath)
        {
            var setting = SceneListGeneratorSetting.LoadFromAsset();
            if (setting == null) return;
            if (!setting.IsEnableAssetPostprocessor) return;
            SceneListGenerator.GenerateByEditorBuildSettings(
                setting.WritePath,
                setting.Namespace
            );
        }
    }
}