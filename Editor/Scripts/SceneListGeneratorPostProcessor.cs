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
                var isSceneUpdate = path.EndsWith(".unity") && path.StartsWith("Assets");
                var isEditorSettingUpdate = path.EndsWith("EditorBuildSettings.asset");
                if (isSceneUpdate || isEditorSettingUpdate)
                {
                    Update();
                }
            }
        }

        private static void Update()
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