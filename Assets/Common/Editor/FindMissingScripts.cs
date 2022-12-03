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
        [MenuItem("Tools/Project/Find Missing Scripts In Project")]
        private static void FindMissingScriptsMenuItem()
        {
            string[] prefabsPaths = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();

            List<Component> components = null;

            foreach (string path in prefabsPaths)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                foreach (Component component in prefab.GetComponentsInChildren<Component>())
                {
                    if (component == null)
                    {
                        components.Add(component);
                        Console.LogWarning($"Prefab «{path}» has missing script ", prefab, DColor.white);
                        break;
                    }
                }
            }

            if (components == null)
            {
                EditorUtility.DisplayDialog("Select Missing Scripts Result", "No missing scripts in project", "Ok");
            }
        }

        [MenuItem("Tools/Project/Find Missing Scripts On Scene")]
        private static void FindMissingScriptsOnSceneMenuItem()
        {
            List<Component> components = null;

            foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
            {
                foreach (Component component in gameObject.GetComponents<Component>())
                {
                    if (component == null)
                    {
                        if (components == null) components = new();
                        components.Add(component);
                        Console.LogWarning($"GameObject «{gameObject.name}» has missing script ", gameObject, DColor.white);
                    }
                }
            }

            if (components == null)
            {
                EditorUtility.DisplayDialog("Select Missing Scripts Result", "No missing scripts in this scene", "Ok");
            }
        }

        [MenuItem("Tools/Project/Find Missing Scripts On All Scenes")]
        private static void FindMissingScriptsOnAllScenesMenuItem()
        {
            string[] scenesPaths = AssetDatabase.GetAllAssetPaths().
                Where(path => path.EndsWith(".unity", System.StringComparison.OrdinalIgnoreCase)).ToArray();

            List<Component> components = null;

            var currentScene = EditorSceneManager.GetActiveScene().path;

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            foreach (string scene in scenesPaths)
            {
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
                            Console.LogWarning($"GameObject «{gameObject.name}» on scene «{_scene.name}» has missing script ", gameObject, DColor.white);
                        }
                    }
                }
            }

            EditorSceneManager.OpenScene(currentScene);

            if (components == null)
            {
                EditorUtility.DisplayDialog("Select Missing Scripts Result", "No missing scripts in project scenes", "Ok");
            }
        }

        [MenuItem("Tools/Project/Select Missing Scipts (Only first founded)")]
        private static void SelectFirstMissingScriptsOnAllScenesMenuItem()
        {
            string[] scenesPaths = AssetDatabase.GetAllAssetPaths().
                Where(path => path.EndsWith(".unity", System.StringComparison.OrdinalIgnoreCase)).ToArray();

            List<GameObject> selection = null;

            var currentScene = EditorSceneManager.GetActiveScene().path;

            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            foreach (string scene in scenesPaths)
            {
                if (selection != null) break;
                EditorSceneManager.OpenScene(scene);
                foreach (GameObject gameObject in GameObject.FindObjectsOfType<GameObject>(true))
                {
                    if (selection != null) break;
                    foreach (Component component in gameObject.GetComponents<Component>())
                    {
                        if (component == null)
                        {
                            if (selection == null) selection = new();
                            selection.Add(gameObject);
                            Selection.objects = selection.ToArray();
                            break;
                        }
                    }
                }
            }

            if (selection == null)
            {
                EditorUtility.DisplayDialog("Select Missing Scripts Result", "No missing scripts in project scenes", "Ok");
                EditorSceneManager.OpenScene(currentScene);
            }
        }
    }
}