using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateScriptMenu
{
    [MenuItem("Assets/Create/Script/MonoBehaviour", priority = 80)]
    private static void CreateMonoBehaviourMenuItem()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create MonoBehviour", GetCurrentPath(), "NewMonoBehaviour.cs", "cs");
        string pathfToTemplate = Application.dataPath + "/Common/Editor/Templates/MonoBehaviour.txt";
        MakeScriptFromTemplate(pathToNewFile, pathfToTemplate);
    }

    [MenuItem("Assets/Create/Script/Empty", priority = 80)]
    private static void CreateEmptyMenuItem()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create Empty", GetCurrentPath(), "NewEmpty.cs", "cs");
        string pathfToTemplate = Application.dataPath + "/Common/Editor/Templates/Empty.txt";
        MakeScriptFromTemplate(pathToNewFile, pathfToTemplate);
    }

    [MenuItem("Assets/Create/Script/Editor", priority = 80)]
    private static void CreateEditorMenuItem()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create Editor", GetCurrentPath(), "NewEditor.cs", "cs");
        string pathfToTemplate = Application.dataPath + "/Common/Editor/Templates/Editor.txt";
        MakeScriptFromTemplate(pathToNewFile, pathfToTemplate);
    }

    [MenuItem("Assets/Create/Script/Plugin", priority = 80)]
    private static void CreatePluginMenuItem()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create Plugin", GetCurrentPath(), "NewPlugin.cs", "cs");
        string pathfToTemplate = Application.dataPath + "/Common/Editor/Templates/Plugin.txt";
        MakeScriptFromTemplate(pathToNewFile, pathfToTemplate);
    }

    [MenuItem("Assets/Create/Script/Util", priority = 80)]
    private static void CreateUtilMenuItem()
    {
        string pathToNewFile = EditorUtility.SaveFilePanel("Create Util", GetCurrentPath(), "NewUtil.cs", "cs");
        string pathfToTemplate = Application.dataPath + "/Common/Editor/Templates/Util.txt";
        MakeScriptFromTemplate(pathToNewFile, pathfToTemplate);
    }

    private static string GetCurrentPath()
    {
        string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        if (path.Contains("."))
        {
            int index = path.LastIndexOf("/");
            path = path.Substring(0, index);
        }
        return path;
    }

    private static void MakeScriptFromTemplate(string pathToNewFile, string pathToTemplate)
    {
        if (!string.IsNullOrWhiteSpace(pathToNewFile))
        {
            FileInfo fileInfo = new FileInfo(pathToNewFile);
            string nameOfScript = Path.GetFileNameWithoutExtension(fileInfo.Name);

            string text = File.ReadAllText(pathToTemplate);

            text = text.Replace("#SCRIPTNAME#", nameOfScript);

            text = text.Replace("#SCRIPTNAMEWITHOUTEDITOR#", nameOfScript.Replace("Editor", ""));

            text = text.Replace("#SCRIPTNAMEVOID#", nameOfScript + "Void");

            File.WriteAllText(pathToNewFile, text);
            AssetDatabase.Refresh();
        }
    }
}