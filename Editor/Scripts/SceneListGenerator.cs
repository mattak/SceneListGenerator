using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace SceneListGenerator.EditorRuntime
{
    public static class SceneListGenerator
    {
        private const string TemplateFilePath =
            "Packages/me.mattak.scenelistgenerator/Editor/Templates/SceneListTemplate.txt";

        public static void GenerateByEditorBuildSettings(string outputPath, string appNamespace)
        {
            var sceneNames = EditorBuildSettings.scenes.Select(x => Path.GetFileNameWithoutExtension(x.path));
            Generate(outputPath, TemplateFilePath, appNamespace, sceneNames);
        }

        public static void Generate(
            string outputPath,
            string templateFile,
            string appNamespace,
            IEnumerable<string> sceneNames
        )
        {
            var fieldsBuilder = new StringBuilder();
            var sceneEnumsBuilder = new StringBuilder();

            var isFirstLine = true;
            foreach (var sceneName in sceneNames)
            {
                if (!isFirstLine)
                {
                    fieldsBuilder.Append("\n");
                    sceneEnumsBuilder.Append("\n");
                }

                isFirstLine = false;
                fieldsBuilder
                    .Append("        public const string ")
                    .Append(sceneName)
                    .Append(" = \"")
                    .Append(sceneName)
                    .Append("\";");
                sceneEnumsBuilder
                    .Append("            ")
                    .Append(sceneName)
                    .Append(",");
            }

            var body = File.ReadAllText(templateFile)
                .Replace("#NAMESPACE#", appNamespace)
                .Replace("#FIELDS#", fieldsBuilder.ToString())
                .Replace("#SCENE_ENUMS#", sceneEnumsBuilder.ToString());

            File.WriteAllText(outputPath, body);
            if (!File.Exists(outputPath))
            {
                AssetDatabase.ImportAsset(outputPath);
            }
        }
    }
}