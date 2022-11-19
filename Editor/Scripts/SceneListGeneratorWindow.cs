using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace SceneListGenerator.EditorRuntime
{
    public class SceneListGeneratorWindow : EditorWindow
    {
        [MenuItem("Window/Scene List Generator")]
        public static void ShowWindow()
        {
            GetWindow<SceneListGeneratorWindow>("Scene List Generator");
        }

        private void OnEnable()
        {
            Bind(rootVisualElement);
        }

        private void Bind(VisualElement root)
        {
            var uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/me.mattak.scenelistgenerator/Editor/Layout/SceneListGeneratorWindow.uxml"
            );
            var ui = uiAsset.CloneTree();
            root.Clear();
            root.Add(ui);

            var generateAllButton = root.Q<Button>("GenerateAllButton");
            var createSettingButton = root.Q<Button>("CreateSettingButton");
            var settingsInspector = root.Q<InspectorElement>("SettingsInspector");

            var serializedObject = FindScriptableObjectSetting();

            if (serializedObject != null)
            {
                generateAllButton.visible = true;
                generateAllButton.style.display = DisplayStyle.Flex;
                settingsInspector.visible = true;
                settingsInspector.style.display = DisplayStyle.Flex;
                createSettingButton.visible = false;
                createSettingButton.style.display = DisplayStyle.None;
                settingsInspector.Bind(serializedObject);
                generateAllButton.clicked += OnClickGenerateAll;
            }
            else
            {
                generateAllButton.visible = false;
                generateAllButton.style.display = DisplayStyle.None;
                settingsInspector.visible = false;
                settingsInspector.style.display = DisplayStyle.None;
                createSettingButton.visible = true;
                createSettingButton.style.display = DisplayStyle.Flex;
                createSettingButton.clicked += OnClickCreateSetting;
            }
        }

        private static void OnClickGenerateAll()
        {
            var settings = SceneListGeneratorSetting.LoadFromAsset();
            if (settings == null) return;
            SceneListGenerator.GenerateByEditorBuildSettings(settings.WritePath, settings.Namespace);
        }

        private static void OnClickCreateSetting()
        {
            var so = CreateInstance<SceneListGeneratorSetting>();
            var path = "Assets/SceneListGeneratorSetting.asset";
            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = so;
        }

        private static SerializedObject FindScriptableObjectSetting()
        {
            var scriptableObject = SceneListGeneratorSetting.LoadFromAsset();
            if (scriptableObject != null) return new SerializedObject(scriptableObject);
            return null;
        }
    }
}