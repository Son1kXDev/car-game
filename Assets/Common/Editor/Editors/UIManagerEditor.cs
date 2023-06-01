using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Game.Scripts.UI;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : BaseEditor
{
    private SerializedProperty _type;
    private SerializedProperty _rewardData;
    private SerializedProperty _rewardPanel;
    private SerializedProperty _settingsPanel;
    private SerializedProperty _pausePanel;

    protected override void OnEnable()
    {
        base.OnEnable();
        _type = serializedObject.FindProperty(nameof(_type));
        _rewardData = serializedObject.FindProperty(nameof(_rewardData));
        _rewardPanel = serializedObject.FindProperty(nameof(_rewardPanel));
        _settingsPanel = serializedObject.FindProperty(nameof(_settingsPanel));
        _pausePanel = serializedObject.FindProperty(nameof(_pausePanel));
    }

    public override void OnInspectorGUI()
    {
        if (!isUnityNamespace)
            HeaderGUI(componentAttribute);
        serializedObject.Update();
        if (GUILayout.Button(_type.enumNames[_type.enumValueFlag].Replace("0", " ")))
        {
            int value = _type.enumValueIndex + 1;
            if (value > 2) value = 0;
            _type.enumValueIndex = value;
        }

        EditorGUILayout.Space(10);
        if (_type.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(_rewardData);
            EditorGUILayout.PropertyField(_rewardPanel);
            EditorGUILayout.PropertyField(_pausePanel);
        }

        if (_type.enumValueIndex != 2) EditorGUILayout.PropertyField(_settingsPanel);
        serializedObject.ApplyModifiedProperties();
    }
}
