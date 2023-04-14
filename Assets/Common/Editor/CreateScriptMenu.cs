using System.Net;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateScriptMenu : EditorWindow
{
    private string scriptName = "NewScript";

    // private ScriptCreationMode scriptCreationMode = ScriptCreationMode.Template;

    private Template currentTemplate = Template.MonoBehaviour;

    static CreateScriptMenu currentWindow;

    [MenuItem("Assets/Create/C# Script with Template", priority = 80)]
    private static void Init()
    {
        currentWindow = GetWindow<CreateScriptMenu>();
        currentWindow.titleContent = new GUIContent("Create Script");
        currentWindow.minSize = new(500f, 200f);
        currentWindow.maxSize = new(500f, 200f);
        currentWindow.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Create a new C# script", EditorStyles.boldLabel);
        scriptName = EditorGUILayout.TextField("Name", scriptName);

        //scriptCreationMode = (ScriptCreationMode)EditorGUILayout.EnumPopup("Script Creation Mode: ", scriptCreationMode);

        //if (scriptCreationMode == ScriptCreationMode.Template)
        //{
        currentTemplate = (Template)EditorGUILayout.EnumPopup("Template: ", currentTemplate);
        //}

        if (GUILayout.Button("Create"))
            CreateScript();

    }

    private void CreateScript()
    {

        string templatePath = "";
        string scriptContent = "";

        // if (scriptCreationMode == ScriptCreationMode.Template)
        // {
        templatePath = currentTemplate switch
        {
            Template.MonoBehaviour => "Assets/Common/Editor/Templates/MonoBehaviour.txt",
            Template.Editor => "Assets/Common/Editor/Templates/Editor.txt",
            Template.Empty => "Assets/Common/Editor/Templates/Empty.txt",
            Template.Util => "Assets/Common/Editor/Templates/Util.txt",
            Template.Plugin => "Assets/Common/Editor/Templates/Plugin.txt",
            _ => "Assets/Common/Editor/Templates/MonoBehaviour.txt"

        };
        scriptContent = File.ReadAllText(templatePath);

        // }
        // else if (scriptCreationMode == ScriptCreationMode.OpenAI)
        // {
        //     string prompt = "Create a new C# script called " + scriptName;
        //     string model = "your-openai-model-id";
        //     int maxTokens = 256;
        //     float temperature = 0.5f;
        //     int topP = 1;

        //     string[] promptTokens = prompt.Split(' ');

        //     string[] generatedTokens = await OpenAI.GenerateTokens(model, promptTokens, maxTokens, temperature, topP);

        //     scriptContent = string.Join(" ", generatedTokens);
        // }

        scriptContent = scriptContent.Replace("#SCRIPTNAME#", scriptName);

        string fileName = scriptName + ".cs";
        string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(filePath))
            filePath = "Assets";
        else if (!File.Exists(filePath))
            filePath = Path.GetDirectoryName(filePath);


        string fullPath = Path.Combine(filePath, fileName);

        if (File.Exists(fullPath))
        {
            Debug.LogWarning($"File already exists at {fullPath}. File not created.");
            return;
        }

        File.WriteAllText(fullPath, scriptContent);
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<MonoScript>(fullPath);
    }
}

public enum Template
{
    MonoBehaviour,
    Editor,
    Empty,
    Util,
    Plugin
}

// public enum ScriptCreationMode
// {
//     Template,
//     OpenAI
// }
