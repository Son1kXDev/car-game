using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Plugins.Editor
{
    [InitializeOnLoad]
    public static class Preloader
    {
        public static bool Enabled
        {
            get { return EditorPrefs.GetBool("Preloader", true); }
            set { EditorPrefs.SetBool("Preloader", value); }
        }

        static Preloader() => EditorApplication.playModeStateChanged += Preload;

        private static void Preload(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            if (!Enabled) return;

            var index = SceneManager.GetActiveScene().buildIndex;
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                if (EditorUtility.DisplayDialog("Load Manager", "Load application from the first scene?", "Yes", "No"))
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    SceneManager.LoadScene(0);
                    SceneManager.LoadScene(index);
                }
            }
        }

        [MenuItem("Tools/Project/Load From First Scene")]
        private static void SetPreloaderStatus()
        {
            Enabled = !Enabled;
        }

        [MenuItem("Tools/Project/Load From First Scene", true)]
        private static bool ValidateSetPreloaderStatus()
        {
            Menu.SetChecked("Tools/Project/Load From First Scene", Enabled);
            return true;
        }
    }
}