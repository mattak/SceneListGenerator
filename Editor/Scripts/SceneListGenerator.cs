using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace SceneListGenerator.EditorRuntime
{
    public static class SceneListGenerator
    {
        // Define your custom generator by using DidReloadScripts
        // e.g.
        //   public static class OnScriptsReloadHandler
        //   {
        //       [DidReloadScripts]
        //       private static void OnScriptsReloaded()
        //       {
        //           SceneListCodeGenerator.GenerateByEditorBuildSettings(
        //               "Assets/Scripts/SceneListGenerated.cs",
        //               "MyNamespace"
        //           );
        //       }
        //   }
        public static void GenerateByEditorBuildSettings(string outputPath, string appNamespace)
        {
            var sceneNames = EditorBuildSettings.scenes.Select(x => Path.GetFileNameWithoutExtension(x.path));
            Generate(outputPath, appNamespace, sceneNames);
        }

        public static void Generate(string outputPath, string appNamespace, IEnumerable<string> sceneNames)
        {
            var dir = Directory.GetParent(outputPath);
            if (dir is {Exists: false}) Directory.CreateDirectory(dir.FullName);

            var names = new List<string>();
            var builder = new StringBuilder();
            builder.Append(@"namespace ").Append(appNamespace);
            builder.Append(@"
{
    public static class SceneList
    {");

            foreach (var name in sceneNames)
            {
                names.Add(name);
                builder.Append("\n        public static readonly string ").Append(name).Append(" = \"")
                    .Append(name)
                    .Append("\";");
            }

            if (names.Count > 0)
            {
                builder.Append(@"

        public static readonly string SceneListEntry = ").Append(names[0]).Append(";");
            }

            builder.Append(@"

        public static readonly string[] SceneListAll = new string[]
        {");
            foreach (var name in names)
            {
                builder.Append("\n            ").Append(name).Append(",");
            }

            builder.Append(@"
        };
    }
}");

            if (File.Exists(outputPath))
            {
                File.WriteAllText(outputPath, builder.ToString());
            }
            else
            {
                File.WriteAllText(outputPath, builder.ToString());
                AssetDatabase.ImportAsset(outputPath);
            }
        }
    }
}