using UnityEngine;
using UnityEditor;
using Utils.Debugger;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Plugins.Editor
{

    public static class FindMissingScripts
    {

        [MenuItem("Tools/Project/Missing Scripts/Find In Prefabs")]
        private static void FindMissingScriptsMenuItem()
        {
            string[] prefabsPaths = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();

            List<Component> components = null;

            int i = 0;
            foreach (string path in prefabsPaths)
            {
                EditorUtility.DisplayProgressBar("Find Missing Scripts", "Finding missing in prefabs...", i / prefabsPaths.Length);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                foreach (Component component in prefab.GetComponentsInChildren<Component>())
                {
                    if (component == null)
                    {
                        if (components == null) components = new();
                        components.Add(component);
                        Console.LogWarning($"Prefab {path} has missing script ", prefab, DColor.white);
                        break;
                    }
                }
                i++;
            }
            EditorUtility.ClearProgressBar();

            if (components == null)
            {
                EditorUtility.DisplayDialog("Select Missing Scripts Result", "No missing scripts in project", "Ok");
            }
        }

        [MenuItem("Tools/Project/Missing Scripts/Find On Scene")]
        private static void FindMissingScriptsOnSceneMenuItem()
        {
            List<Component> components = null;

            int length = GameObject.FindObjectsOfType<GameObject>(true).Length;
            int i = 0;
            foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
            {
                EditorUtility.DisplayProgressBar("Find Missing Scripts", "Finding missing on current scene...", i / length);
                foreach (Component component in gameObject.GetComponents<Component>())
                {
                    if (component == null)
                    {
                        if (components == null) components = new();
                        components.Add(component);
                        Console.LogWarning($"GameObject {gameObject.name} has missing script ", gameObject, DColor.white);
                    }
                }
                i++;
            }

            EditorUtility.ClearProgressBar();

            if (components == null)
            { EditorUtility.DisplayDialog("Select Missing Scripts Result", "No missing scripts in this scene", "Ok"); }
        }

        [MenuItem("Tools/Project/Missing Scripts/Find On All Scenes")]
        private static void FindMissingScriptsOnAllScenesMenuItem()
        {
            string[] scenesPaths = AssetDatabase.GetAllAssetPaths()
            .Where(path => !path.Contains("Packages", System.StringComparison.OrdinalIgnoreCase))
            .Where(path => path.EndsWith(".unity", System.StringComparison.OrdinalIgnoreCase)).ToArray();

            List<Component> components = null;

            var currentScene = EditorSceneManager.GetActiveScene().path;

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            float progress = 0;
            int i = 0;

            foreach (string scene in scenesPaths)
            {
                i++;
                progress = i / scenesPaths.Length;
                EditorUtility.DisplayProgressBar("Find Missing Scripts", "Finding missing scripts on all scenes...", progress);
                EditorSceneManager.OpenScene(scene);
                foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
                {
                    foreach (Component component in gameObject.GetComponents<Component>())
                    {
                        if (component == null)
                        {
                            if (components == null) components = new();
                            components.Add(component);
                            Scene _scene = gameObject.scene;
                            Console.LogWarning($"GameObject {gameObject.name} on scene {_scene.path} has missing script ", gameObject, DColor.white);
                        }
                    }
                }
            }

            EditorUtility.ClearProgressBar();
            EditorSceneManager.OpenScene(currentScene);

            if (components == null)
            { EditorUtility.DisplayDialog("Select Missing Scripts Result", "No missing scripts in project scenes", "Ok"); }
        }

        [MenuItem("Tools/Project/Missing Scripts/Delete In Prefabs")]
        private static void DeleteMissingScriptsMenuItem()
        {
            string[] prefabsPaths = AssetDatabase.GetAllAssetPaths()
            .Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();

            List<Component> components = null;

            int i = 0;
            foreach (string path in prefabsPaths)
            {
                EditorUtility.DisplayProgressBar("Delete Missing Scripts", "Delete missing in prefabs...", i / prefabsPaths.Length);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                foreach (Component component in prefab.GetComponentsInChildren<Component>())
                {
                    if (component == null)
                    {
                        if (components == null) components = new();
                        components.Add(component);
                        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(prefab);
                        Console.LogWarning($"Prefab {path} missing script was deleted.", prefab, DColor.white);
                        break;
                    }
                }
                i++;
            }
            EditorUtility.ClearProgressBar();

            if (components == null)
            { EditorUtility.DisplayDialog("Delete Missing Scripts Result", "No missing scripts in project", "Ok"); }
        }

        [MenuItem("Tools/Project/Missing Scripts/Delete On Scene")]
        private static void DeleteMissingScriptsOnSceneMenuItem()
        {
            List<Component> components = null;

            int length = GameObject.FindObjectsOfType<GameObject>(true).Length;
            int i = 0;
            foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
            {
                EditorUtility.DisplayProgressBar("Delete Missing Scripts", "Delete missing on current scene...", i / length);
                foreach (Component component in gameObject.GetComponents<Component>())
                {
                    if (component == null)
                    {
                        if (components == null) components = new();
                        components.Add(component);
                        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                        Console.LogWarning($"GameObject {gameObject.name} missing script was deleted.", gameObject, DColor.white);
                    }
                }
                i++;
            }
            EditorUtility.ClearProgressBar();

            if (components == null)
            { EditorUtility.DisplayDialog("Delete Missing Scripts Result", "No missing scripts in this scene", "Ok"); }
        }


    }
}