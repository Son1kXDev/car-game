using UnityEngine;
using UnityEditor;
using Assets.Game.Scripts.Game;
using System;

[CustomEditor(typeof(CarVisual))]
[Obsolete]
public class CarVisualEditor : BaseEditor
{
    #region SerializedProperties
    //SerializedProperty _property;
    SerializedProperty _carBase;
    SerializedProperty _carBack;
    SerializedProperty _carElements;
    SerializedProperty _carOptics;
    SerializedProperty _carSticker;
    SerializedProperty _tires;
    SerializedProperty _rims;
    SerializedProperty _spoiler;
    SerializedProperty _splitter;
    SerializedProperty _lowBeam;
    SerializedProperty _highBeam;
    SerializedProperty _backCasualLights;
    SerializedProperty _backMoveLights;
    SerializedProperty _brakeLights;
    SerializedProperty _lightSwitchSound;

    #endregion

    GUIStyle labelCenteredStyle;


    enum DisplayingPropertyType { Sprites, Lights, Audio }
    DisplayingPropertyType _property;

    protected override void OnEnable()
    {
        base.OnEnable();
        _carBase = serializedObject.FindProperty(nameof(_carBase));
        _carBack = serializedObject.FindProperty(nameof(_carBack));
        _carElements = serializedObject.FindProperty(nameof(_carElements));
        _carOptics = serializedObject.FindProperty(nameof(_carOptics));
        _carSticker = serializedObject.FindProperty(nameof(_carSticker));
        _tires = serializedObject.FindProperty(nameof(_tires));
        _rims = serializedObject.FindProperty(nameof(_rims));
        _spoiler = serializedObject.FindProperty(nameof(_spoiler));
        _splitter = serializedObject.FindProperty(nameof(_splitter));
        _lowBeam = serializedObject.FindProperty(nameof(_lowBeam));
        _highBeam = serializedObject.FindProperty(nameof(_highBeam));
        _backCasualLights = serializedObject.FindProperty(nameof(_backCasualLights));
        _backMoveLights = serializedObject.FindProperty(nameof(_backMoveLights));
        _brakeLights = serializedObject.FindProperty(nameof(_brakeLights));
        _lightSwitchSound = serializedObject.FindProperty(nameof(_lightSwitchSound));
    }
    public override void OnInspectorGUI()
    {
        if (!isUnityNamespace)
            HeaderGUI(componentAttribute);

        if (labelCenteredStyle == null)
        {
            labelCenteredStyle = new(GUI.skin.label);
            labelCenteredStyle.alignment = TextAnchor.MiddleCenter;
        }

        serializedObject.Update();
        _property = (DisplayingPropertyType)EditorGUILayout.EnumPopup(_property);
        GUILayout.Space(10);
        switch (_property)
        {
            case DisplayingPropertyType.Sprites:
                DrawSprites();
                break;
            case DisplayingPropertyType.Lights:
                DrawLights();
                break;
            case DisplayingPropertyType.Audio:
                DrawAudio();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSprites()
    {
        if (AllDataIsSpecified() == false)
            EditorGUILayout.HelpBox("Some variables are not specified", MessageType.Warning);

        _carBase.PropertyField(_carBase.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _carBack.PropertyField(_carBack.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _carElements.PropertyField(_carElements.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _carOptics.PropertyField(_carOptics.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _carSticker.PropertyField(_carSticker.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _spoiler.PropertyField(_spoiler.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _splitter.PropertyField(_splitter.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        DrawTires();
        DrawRims();

    }
    private void DrawLights()
    {
        _lowBeam.PropertyField(_lowBeam.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _highBeam.PropertyField(_highBeam.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _backCasualLights.PropertyField(_backCasualLights.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _backMoveLights.PropertyField(_backMoveLights.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
        _brakeLights.PropertyField(_brakeLights.objectReferenceValue != null ? PropertyIconType.Confirm : PropertyIconType.Error);
    }
    private void DrawAudio()
    { EditorGUILayout.PropertyField(_lightSwitchSound); }
    private bool AllDataIsSpecified()
    {
        bool allDataIsSpecified = _carBase.objectReferenceValue != null && _carBack.objectReferenceValue != null &&
                    _carElements.objectReferenceValue != null && _carOptics.objectReferenceValue != null &&
                    _tires.arraySize != 0 && _rims.arraySize != 0 &&
                    _spoiler.objectReferenceValue != null && _splitter.objectReferenceValue != null;

        for (int i = 0; i < _tires.arraySize; i++)
        {
            SerializedProperty element = _tires.GetArrayElementAtIndex(i);
            if (element.objectReferenceValue == null) return false;
        }

        for (int i = 0; i < _rims.arraySize; i++)
        {
            SerializedProperty element = _rims.GetArrayElementAtIndex(i);
            if (element.objectReferenceValue == null) return false;
        }

        return allDataIsSpecified;
    }
    private void DrawTires()
    {
        _tires.arraySize = 2;
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        bool dataSpecified = true;
        for (int i = 0; i < _tires.arraySize; i++)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(i == 0 ? "Back Tire" : "Front Tire", labelCenteredStyle, GUILayout.Width(Screen.width / 2 * .85f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            SerializedProperty element = _tires.GetArrayElementAtIndex(i);
            element.objectReferenceValue =
            EditorGUILayout.ObjectField(element.objectReferenceValue, typeof(Sprite),
            GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.Width(Screen.width / 2 * .85f));
            if (element.objectReferenceValue == null) dataSpecified = false;
            EditorGUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("");
        dataSpecified.BooleanEditorIcon();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
    private void DrawRims()
    {
        _rims.arraySize = 2;
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        bool dataSpecified = true;
        for (int i = 0; i < _rims.arraySize; i++)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(i == 0 ? "Back Rim" : "Front Rim", labelCenteredStyle, GUILayout.Width(Screen.width / 2 * .85f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            SerializedProperty element = _rims.GetArrayElementAtIndex(i);
            element.objectReferenceValue =
            EditorGUILayout.ObjectField(element.objectReferenceValue, typeof(Sprite),
            GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.MaxWidth(Screen.width / 2 * .85f));
            if (element.objectReferenceValue == null) dataSpecified = false;
            EditorGUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("");
        dataSpecified.BooleanEditorIcon();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

}
