using UnityEditor;
using Plugins.Web;

namespace Editors
{
    [CustomEditor(typeof(OpenWebURL))]
    public class OpenWebURLEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var myScript = target as OpenWebURL;

            myScript.URL = EditorGUILayout.TextField("URL", myScript.URL);

            if (string.IsNullOrEmpty(myScript.URL) || string.IsNullOrWhiteSpace(myScript.URL))
            {
                EditorGUILayout.HelpBox("URL can't be empty", MessageType.Error);
                return;
            }

            if (!myScript.URL.Contains("https://"))
            {
                EditorGUILayout.HelpBox("URL must starts with https://", MessageType.Warning);
            }
        }
    }
}