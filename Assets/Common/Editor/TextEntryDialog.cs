using UnityEngine;
using UnityEditor;


namespace Editors
{
    public class TextEntryDialog : EditorWindow
    {
        private string m_entryFieldResult;

        private static string _lable;

        public static string Show(string windowName, string lable)
        {
            TextEntryDialog dialog = CreateInstance<TextEntryDialog>();
            dialog.titleContent = new GUIContent(windowName);

            _lable = lable;

            dialog.maxSize = new Vector2(320, 120);
            dialog.minSize = new Vector2(320, 120);

            dialog.m_entryFieldResult = string.Empty;

            dialog.ShowModal();

            return dialog.m_entryFieldResult;
        }

        private void OnGUI()
        {
            GUILayout.Space(20);

            GUILayout.Label(_lable);

            m_entryFieldResult = EditorGUILayout.TextField(m_entryFieldResult, GUILayout.Width(310));

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Cansel", GUILayout.Width(100)))
            {
                Cansel();
            }

            if (GUILayout.Button("OK", GUILayout.Width(100)))
            {
                Accept();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(20);
        }

        private void Accept()
        {
            if (!string.IsNullOrEmpty(m_entryFieldResult))
            {
                Close();
            }
        }

        private void Cansel()
        {
            m_entryFieldResult = string.Empty;
            Close();
        }
    }
}