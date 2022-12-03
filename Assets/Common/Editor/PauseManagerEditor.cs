using UnityEngine;
using UnityEditor;
using Plugins.Pause;
using System.Collections.Generic;

namespace Editors
{
    [CustomEditor(typeof(PauseManager))]
    public class PauseManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            PauseManager pauseManager = (PauseManager)target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Type"));

            if (pauseManager.Type == PauseType._switch || pauseManager.Type == PauseType._switchWithPanel)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PauseButton"));
                if (pauseManager.PauseButton == null)
                {
                    EditorGUILayout.HelpBox("Pause Button can't be null", MessageType.Error);
                }
            }

            if (pauseManager.Type == PauseType._single || pauseManager.Type == PauseType._switchWithPanel)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PausePanelPopup"));
                if (pauseManager.PausePanelPopup == null)
                {
                    EditorGUILayout.HelpBox("Pause Panel can't be null", MessageType.Error);
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            pauseManager.showMonos = EditorGUILayout.Foldout(pauseManager.showMonos,
                new GUIContent("Scripts", "Scripts that will be disabled on pause"), true);

            List<MonoBehaviour> list = pauseManager.monosToPause;

            int size = list.Count;

            if (pauseManager.showMonos)
            {
                size = Mathf.Max(0, EditorGUILayout.IntField(list.Count));
                if (GUILayout.Button(new GUIContent("-", "Remove")) && size > 0) size--;
                if (GUILayout.Button(new GUIContent("+", "Add"))) size++;
            }

            EditorGUILayout.EndHorizontal();

            if (pauseManager.showMonos)
            {
                EditorGUILayout.Space();
                if (size == 0) EditorGUILayout.HelpBox("List is empty", MessageType.Info);
                EditorGUI.indentLevel++;
                while (size > list.Count) list.Add(null);
                while (size < list.Count) list.RemoveAt(list.Count - 1);

                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = EditorGUILayout.ObjectField("Script " + (i + 1), list[i], typeof(MonoBehaviour), true) as MonoBehaviour;
                }
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}