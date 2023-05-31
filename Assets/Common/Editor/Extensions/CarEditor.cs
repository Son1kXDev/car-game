using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;


[CustomEditor(typeof(Car))]
[Obsolete]
public class CarEditor : Editor
{
    #region SerializedProperties
    SerializedProperty _id;
    SerializedProperty _property;
    //variables
    SerializedProperty _acceleration;
    SerializedProperty _maxSpeed;
    SerializedProperty _maxBackSpeed;
    SerializedProperty _brakeForce;
    SerializedProperty _airBrakeForce;
    SerializedProperty _gearBrakeForce;
    SerializedProperty _wheelSize;
    SerializedProperty _suspensionFrequency;
    SerializedProperty _suspensionHeight;

    //gear
    SerializedProperty _gearType;
    SerializedProperty _gearsMaxSpeed;
    SerializedProperty _maximumMotorForces;

    //sprites
    SerializedProperty _baseSprite;
    SerializedProperty _backSprite;
    SerializedProperty _elementsSprite;
    SerializedProperty _opticsSprite;
    SerializedProperty _tiresSprites;
    SerializedProperty _rimsSprites;
    SerializedProperty _spoilersSprites;
    SerializedProperty _splittersSprites;

    //icons
    SerializedProperty _tiresIconsSprites;
    SerializedProperty _rimsIconsSprites;
    SerializedProperty _spoilersIconsSprites;
    SerializedProperty _splittersIconsSprites;

    //information
    SerializedProperty _name;
    SerializedProperty _cost;
    SerializedProperty _weight;
    SerializedProperty _strength;
    SerializedProperty _tiresNames;
    SerializedProperty _tiresCost;
    SerializedProperty _rimsNames;
    SerializedProperty _rimsCost;
    SerializedProperty _spoilersNames;
    SerializedProperty _spoilersCost;
    SerializedProperty _splittersNames;
    SerializedProperty _splittersCost;
    #endregion

    private AnimBool selected = new();

    private AnimBool showTires = new();
    private AnimBool showRims = new();
    private AnimBool showSpoilers = new();
    private AnimBool showSplitters = new();

    private bool _showTires
    {
        get { return bool.Parse(EditorPrefs.GetString("tires", "true")); }
        set { EditorPrefs.SetString("tires", value.ToString()); }
    }
    private bool _showRims
    {
        get { return bool.Parse(EditorPrefs.GetString("rims", "true")); }
        set { EditorPrefs.SetString("rims", value.ToString()); }
    }
    private bool _showSpoilers
    {
        get { return bool.Parse(EditorPrefs.GetString("spoilers", "true")); }
        set { EditorPrefs.SetString("spoilers", value.ToString()); }
    }
    private bool _showSplitters
    {
        get { return bool.Parse(EditorPrefs.GetString("splitters", "true")); }
        set { EditorPrefs.SetString("splitters", value.ToString()); }
    }

    private bool _accelerationHelpBox = false;
    private bool _maxSpeedHelpBox = false;
    private bool _maxBackSpeedHelpBox = false;
    private bool _brakeForceHelpBox = false;
    private bool _airBrakeForceHelpBox = false;
    private bool _gearBrakeForceHelpBox = false;
    private bool _suspensionFrequencyHelpBox = false;
    private bool _suspensionHeightHelpBox = false;
    private bool _wheelSizeHelpBox
    {
        get { return bool.Parse(EditorPrefs.GetString("wheelSizeHelpBox", "true")); }
        set { EditorPrefs.SetString("wheelSizeHelpBox", value.ToString()); }
    }

    private int oldIndex;
    private float maxWidth = 225;
    private void OnEnable()
    {
        _id = serializedObject.FindProperty(nameof(_id));
        _property = serializedObject.FindProperty(nameof(_property));
        _acceleration = serializedObject.FindProperty(nameof(_acceleration));
        _maxSpeed = serializedObject.FindProperty(nameof(_maxSpeed));
        _maxBackSpeed = serializedObject.FindProperty(nameof(_maxBackSpeed));
        _brakeForce = serializedObject.FindProperty(nameof(_brakeForce));
        _airBrakeForce = serializedObject.FindProperty(nameof(_airBrakeForce));
        _gearBrakeForce = serializedObject.FindProperty(nameof(_gearBrakeForce));
        _wheelSize = serializedObject.FindProperty(nameof(_wheelSize));
        _suspensionFrequency = serializedObject.FindProperty(nameof(_suspensionFrequency));
        _suspensionHeight = serializedObject.FindProperty(nameof(_suspensionHeight));
        _gearType = serializedObject.FindProperty(nameof(_gearType));
        _gearsMaxSpeed = serializedObject.FindProperty(nameof(_gearsMaxSpeed));
        _maximumMotorForces = serializedObject.FindProperty(nameof(_maximumMotorForces));
        _baseSprite = serializedObject.FindProperty(nameof(_baseSprite));
        _backSprite = serializedObject.FindProperty(nameof(_backSprite));
        _elementsSprite = serializedObject.FindProperty(nameof(_elementsSprite));
        _opticsSprite = serializedObject.FindProperty(nameof(_opticsSprite));
        _tiresSprites = serializedObject.FindProperty(nameof(_tiresSprites));
        _rimsSprites = serializedObject.FindProperty(nameof(_rimsSprites));
        _spoilersSprites = serializedObject.FindProperty(nameof(_spoilersSprites));
        _splittersSprites = serializedObject.FindProperty(nameof(_splittersSprites));
        _tiresIconsSprites = serializedObject.FindProperty(nameof(_tiresIconsSprites));
        _rimsIconsSprites = serializedObject.FindProperty(nameof(_rimsIconsSprites));
        _spoilersIconsSprites = serializedObject.FindProperty(nameof(_spoilersIconsSprites));
        _splittersIconsSprites = serializedObject.FindProperty(nameof(_splittersIconsSprites));
        _name = serializedObject.FindProperty(nameof(_name));
        _cost = serializedObject.FindProperty(nameof(_cost));
        _weight = serializedObject.FindProperty(nameof(_weight));
        _strength = serializedObject.FindProperty(nameof(_strength));
        _tiresNames = serializedObject.FindProperty(nameof(_tiresNames));
        _tiresCost = serializedObject.FindProperty(nameof(_tiresCost));
        _rimsNames = serializedObject.FindProperty(nameof(_rimsNames));
        _rimsCost = serializedObject.FindProperty(nameof(_rimsCost));
        _spoilersNames = serializedObject.FindProperty(nameof(_spoilersNames));
        _spoilersCost = serializedObject.FindProperty(nameof(_spoilersCost));
        _splittersNames = serializedObject.FindProperty(nameof(_splittersNames));
        _splittersCost = serializedObject.FindProperty(nameof(_splittersCost));

        selected.valueChanged.AddListener(Repaint);
        showTires.valueChanged.AddListener(Repaint);
        showRims.valueChanged.AddListener(Repaint);
        showSpoilers.valueChanged.AddListener(Repaint);
        showSplitters.valueChanged.AddListener(Repaint);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.TextField("ID", _id.stringValue);
        if (_id.stringValue == string.Empty)
        {
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Generate ID"))
                GenerateGuid();
            serializedObject.ApplyModifiedProperties();
            return;
        }
        EditorGUILayout.Space(10);
        oldIndex = _property.enumValueIndex;
        EditorGUILayout.PropertyField(_property);
        EditorGUILayout.Space(10);

        selected.target = oldIndex == _property.enumValueIndex;
        if (EditorGUILayout.BeginFadeGroup(selected.faded))
            switch (Enum.GetValues(typeof(SerializedPropertyType)).GetValue(_property.enumValueIndex))
            {
                case SerializedPropertyType.Variables:
                    DrawVariables();
                    break;
                case SerializedPropertyType.Gear:
                    DrawGear();
                    break;
                case SerializedPropertyType.Data:
                    DrawData();
                    break;
                case SerializedPropertyType.Information:
                    DrawInfo();
                    break;
            }
        EditorGUILayout.EndFadeGroup();
        if (AllDataIsSpecified() == false)
            EditorGUILayout.HelpBox("Some variables are not specified", MessageType.Error);

        serializedObject.ApplyModifiedProperties();
    }
    private void GenerateGuid()
    { _id.stringValue = System.Guid.NewGuid().ToString(); }
    private void DrawVariables()
    {
        _accelerationHelpBox = _acceleration.PropertyField(_acceleration.floatValue <= 0 ? PropertyIconType.Error : PropertyIconType.Confirm,
        _accelerationHelpBox, "Wheel acceleration speed.");
        _maxSpeedHelpBox = _maxSpeed.PropertyField(_maxSpeed.floatValue <= 0 ? PropertyIconType.Error : PropertyIconType.Confirm,
        _maxSpeedHelpBox, "The maximum allowable wheel speed. \nDo nothing now, because of Gear / Gears Max Speed");
        _maxBackSpeedHelpBox = _maxBackSpeed.PropertyField(_maxBackSpeed.floatValue <= 0 ? PropertyIconType.Error : PropertyIconType.Confirm,
        _maxBackSpeedHelpBox, "The maximum allowable reverse wheel speed.");
        _brakeForceHelpBox = _brakeForce.PropertyField(_brakeForce.floatValue <= 0 ? PropertyIconType.Error : PropertyIconType.Confirm,
        _brakeForceHelpBox, "Deceleration speed when the brake button is pressed.");
        _airBrakeForceHelpBox = _airBrakeForce.PropertyField(_airBrakeForce.floatValue <= 0 ? PropertyIconType.Error : PropertyIconType.Confirm,
        _airBrakeForceHelpBox, "Deceleration speed due to air friction.");
        _gearBrakeForceHelpBox = _gearBrakeForce.PropertyField(_gearBrakeForce.floatValue <= 0 ? PropertyIconType.Error : PropertyIconType.Confirm,
        _gearBrakeForceHelpBox, "Deceleration speed when changing gear.");
        _suspensionFrequencyHelpBox = _suspensionFrequency.PropertyField(_suspensionFrequency.floatValue == 0 ? PropertyIconType.Error : PropertyIconType.Confirm,
        _suspensionFrequencyHelpBox, "Default value for the car's suspension frequency.");
        _suspensionHeightHelpBox = _suspensionHeight.PropertyField(_suspensionHeight.floatValue == 0 ? PropertyIconType.Error : PropertyIconType.Confirm,
        _suspensionHeightHelpBox, "Default value for the car's suspension height.");

        string helpBoxMessage = "Assign this config to your prefab's CarConfig.cs script to fine-tune the Wheel Size value.";
        helpBoxMessage += "\nIf you already have this config set to your Car prefab, simply select your Car object in inspector";
        helpBoxMessage += " and it will automatically show your gizmos for Wheel Size value.";
        Texture2D texture = (Texture2D)EditorGUIUtility.Load("Assets/Resources/Editor/WheelSizePreview.png");
        _wheelSizeHelpBox = _wheelSize.PropertyField(_wheelSize.floatValue == 0 ? PropertyIconType.Error : PropertyIconType.Confirm,
        _wheelSizeHelpBox, helpBoxMessage, texture);
    }

    private void DrawGear()
    {
        _gearType.PropertyField(PropertyIconType.Warning);
        _gearsMaxSpeed.PropertyField(PropertyIconType.Warning);
        _maximumMotorForces.PropertyField(PropertyIconType.Warning);
        EditorGUILayout.Space(10);
        EditorGUILayout.HelpBox("These parameters are deprecated and will be removed in the future.", MessageType.Warning);
    }

    private void DrawData()
    {
        _baseSprite.PropertyField(_baseSprite.objectReferenceValue == null ? PropertyIconType.Error : PropertyIconType.Confirm);
        _backSprite.PropertyField(_backSprite.objectReferenceValue == null ? PropertyIconType.Error : PropertyIconType.Confirm);
        _elementsSprite.PropertyField(_elementsSprite.objectReferenceValue == null ? PropertyIconType.Error : PropertyIconType.Confirm);
        _opticsSprite.PropertyField(_opticsSprite.objectReferenceValue == null ? PropertyIconType.Error : PropertyIconType.Confirm);
        EditorGUILayout.Space(10);
        DrawTires();
        DrawRims();
        DrawSpoilers();
        DrawSplitters();

        EditorGUILayout.Space(10);
    }

    private bool AllDataIsSpecified()
    {
        return _baseSprite.objectReferenceValue.IsNull() == false && _backSprite.objectReferenceValue.IsNull() == false &&
                    _elementsSprite.objectReferenceValue.IsNull() == false && _opticsSprite.objectReferenceValue.IsNull() == false &&
                    TiresDataIsSpecified() && RimsDataIsSpecified() && SpoilersDataIsSpecified() && SplitterDataIsSpecified() &&
                    _name.stringValue.IsEmpty() == false && _cost.floatValue.IsGreater(0) && _weight.floatValue.IsGreater(0)
                    && _strength.floatValue.IsGreater(0) && _acceleration.floatValue.IsGreater(0)
                    && _maxSpeed.floatValue.IsGreater(0) && _maxBackSpeed.floatValue.IsGreater(0)
                    && _brakeForce.floatValue.IsGreater(0) && _airBrakeForce.floatValue.IsGreater(0)
                    && _gearBrakeForce.floatValue.IsGreater(0) && _suspensionFrequency.floatValue.IsGreater(0)
                    && _suspensionHeight.floatValue.IsEqual(0) == false && _wheelSize.floatValue.IsGreater(0);
    }

    private bool TiresDataIsSpecified()
    {
        for (int i = 0; i < _tiresNames.arraySize; i++)
        {
            SerializedProperty element = _tiresNames.GetArrayElementAtIndex(i);
            SerializedProperty element3 = _tiresSprites.GetArrayElementAtIndex(i);
            SerializedProperty element4 = _tiresIconsSprites.GetArrayElementAtIndex(i);

            if (element.stringValue.IsEmpty()) return false;
            if (element3.objectReferenceValue.IsNull()) return false;
            if (element4.objectReferenceValue.IsNull()) return false;
        }
        return _tiresNames.arraySize > 0;
    }

    private bool RimsDataIsSpecified()
    {
        for (int i = 0; i < _rimsNames.arraySize; i++)
        {
            SerializedProperty element = _rimsNames.GetArrayElementAtIndex(i);
            SerializedProperty element3 = _rimsSprites.GetArrayElementAtIndex(i);
            SerializedProperty element4 = _rimsIconsSprites.GetArrayElementAtIndex(i);

            if (element.stringValue.IsEmpty()) return false;
            if (element3.objectReferenceValue.IsNull()) return false;
            if (element4.objectReferenceValue.IsNull()) return false;
        }
        return _rimsNames.arraySize > 0;
    }

    private bool SpoilersDataIsSpecified()
    {
        for (int i = 0; i < _spoilersNames.arraySize; i++)
        {
            SerializedProperty element = _spoilersNames.GetArrayElementAtIndex(i);
            SerializedProperty element3 = _spoilersSprites.GetArrayElementAtIndex(i);
            SerializedProperty element4 = _spoilersIconsSprites.GetArrayElementAtIndex(i);

            if (element.stringValue.IsEmpty()) return false;
            if (element3.objectReferenceValue.IsNull()) return false;
            if (element4.objectReferenceValue.IsNull()) return false;
        }
        return _spoilersNames.arraySize > 0;
    }

    private bool SplitterDataIsSpecified()
    {
        for (int i = 0; i < _splittersNames.arraySize; i++)
        {
            SerializedProperty element = _splittersNames.GetArrayElementAtIndex(i);
            SerializedProperty element3 = _splittersSprites.GetArrayElementAtIndex(i);
            SerializedProperty element4 = _splittersIconsSprites.GetArrayElementAtIndex(i);

            if (element.stringValue.IsEmpty()) return false;
            if (element3.objectReferenceValue.IsNull()) return false;
            if (element4.objectReferenceValue.IsNull()) return false;
        }
        return _splittersNames.arraySize > 0;
    }

    private void DrawInfo()
    {
        _name.PropertyField(_name.stringValue == string.Empty ? PropertyIconType.Error : PropertyIconType.Confirm);
        _cost.PropertyField(_cost.floatValue <= 0 ? PropertyIconType.Error : PropertyIconType.Confirm);
        _weight.PropertyField(_weight.floatValue <= 0 ? PropertyIconType.Error : PropertyIconType.Confirm);
        _strength.PropertyField(_strength.floatValue <= 0 ? PropertyIconType.Error : PropertyIconType.Confirm);

        EditorGUILayout.Space(10);
    }

    private void DrawTires()
    {
        EditorGUILayout.BeginHorizontal();
        _showTires = showTires.target = EditorGUILayout.BeginFoldoutHeaderGroup(_showTires, "Tires");
        Color color = GUI.backgroundColor;
        TiresDataIsSpecified().BooleanEditorIcon();
        EditorGUILayout.EndHorizontal();
        if (EditorGUILayout.BeginFadeGroup(showTires.faded))
        {
            GUI.backgroundColor = color;
            for (int i = 0; i < _tiresNames.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty element = _tiresNames.GetArrayElementAtIndex(i);
                SerializedProperty element2 = _tiresCost.GetArrayElementAtIndex(i);
                SerializedProperty element3 = _tiresSprites.GetArrayElementAtIndex(i);
                SerializedProperty element4 = _tiresIconsSprites.GetArrayElementAtIndex(i);
                bool allRight = (element.stringValue != string.Empty
                && element3.objectReferenceValue != null
                && element4.objectReferenceValue != null);
                if (element.stringValue == string.Empty) GUI.backgroundColor = Color.red;
                element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                GUI.backgroundColor = color;
                element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                if (element3.objectReferenceValue == null) GUI.backgroundColor = Color.red;
                element3.objectReferenceValue = EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.backgroundColor = color;
                if (element4.objectReferenceValue == null) GUI.backgroundColor = Color.red;
                element4.objectReferenceValue = EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.backgroundColor = color;
                allRight.BooleanEditorIcon();
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(1);
            EditorGUILayout.BeginHorizontal();
            if (_tiresNames.arraySize == 0 || GUILayout.Button("+", GUILayout.MaxWidth(50)))
            {
                _tiresNames.arraySize++;
                _tiresCost.arraySize++;
                _tiresSprites.arraySize++;
                _tiresIconsSprites.arraySize++;
                _tiresNames.GetArrayElementAtIndex(_tiresNames.arraySize - 1).stringValue = string.Empty;
                _tiresCost.GetArrayElementAtIndex(_tiresCost.arraySize - 1).intValue = 0;
                _tiresSprites.GetArrayElementAtIndex(_tiresSprites.arraySize - 1).objectReferenceValue = null;
                _tiresIconsSprites.GetArrayElementAtIndex(_tiresIconsSprites.arraySize - 1).objectReferenceValue = null;
            }
            GUI.enabled = _tiresNames.arraySize > 1;
            if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
            {
                _tiresNames.arraySize--;
                _tiresCost.arraySize--;
                _tiresSprites.arraySize--;
                _tiresIconsSprites.arraySize--;
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);
    }
    private void DrawRims()
    {
        EditorGUILayout.BeginHorizontal();
        _showRims = showRims.target = EditorGUILayout.BeginFoldoutHeaderGroup(_showRims, "Rims");
        Color color = GUI.backgroundColor;
        RimsDataIsSpecified().BooleanEditorIcon();
        EditorGUILayout.EndHorizontal();
        if (EditorGUILayout.BeginFadeGroup(showRims.faded))
        {
            for (int i = 0; i < _rimsNames.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty element = _rimsNames.GetArrayElementAtIndex(i);
                SerializedProperty element2 = _rimsCost.GetArrayElementAtIndex(i);
                SerializedProperty element3 = _rimsSprites.GetArrayElementAtIndex(i);
                SerializedProperty element4 = _rimsIconsSprites.GetArrayElementAtIndex(i);
                bool allRight = (element.stringValue != string.Empty
                && element3.objectReferenceValue != null
                && element4.objectReferenceValue != null);
                if (element.stringValue == string.Empty) GUI.backgroundColor = Color.red;
                element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                GUI.backgroundColor = color;
                element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                if (element3.objectReferenceValue == null) GUI.backgroundColor = Color.red;
                element3.objectReferenceValue = EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.backgroundColor = color;
                if (element4.objectReferenceValue == null) GUI.backgroundColor = Color.red;
                element4.objectReferenceValue = EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.backgroundColor = color;
                allRight.BooleanEditorIcon();
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(1);
            EditorGUILayout.BeginHorizontal();
            if (_rimsNames.arraySize == 0 || GUILayout.Button("+", GUILayout.MaxWidth(50)))
            {
                _rimsNames.arraySize++;
                _rimsCost.arraySize++;
                _rimsSprites.arraySize++;
                _rimsIconsSprites.arraySize++;
                _rimsNames.GetArrayElementAtIndex(_rimsNames.arraySize - 1).stringValue = string.Empty;
                _rimsCost.GetArrayElementAtIndex(_rimsCost.arraySize - 1).intValue = 0;
                _rimsSprites.GetArrayElementAtIndex(_rimsSprites.arraySize - 1).objectReferenceValue = null;
                _rimsIconsSprites.GetArrayElementAtIndex(_rimsIconsSprites.arraySize - 1).objectReferenceValue = null;
            }
            GUI.enabled = _rimsNames.arraySize > 1;
            if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
            {
                _rimsNames.arraySize--;
                _rimsCost.arraySize--;
                _rimsSprites.arraySize--;
                _rimsIconsSprites.arraySize--;
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);
    }
    private void DrawSpoilers()
    {
        EditorGUILayout.BeginHorizontal();
        _showSpoilers = showSpoilers.target = EditorGUILayout.BeginFoldoutHeaderGroup(_showSpoilers, "Spoilers");
        Color color = GUI.backgroundColor;
        SpoilersDataIsSpecified().BooleanEditorIcon();
        EditorGUILayout.EndHorizontal();
        if (EditorGUILayout.BeginFadeGroup(showSpoilers.faded))
        {
            for (int i = 0; i < _spoilersNames.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty element = _spoilersNames.GetArrayElementAtIndex(i);
                SerializedProperty element2 = _spoilersCost.GetArrayElementAtIndex(i);
                SerializedProperty element3 = _spoilersSprites.GetArrayElementAtIndex(i);
                SerializedProperty element4 = _spoilersIconsSprites.GetArrayElementAtIndex(i);
                bool allRight = (element.stringValue != string.Empty
                && element3.objectReferenceValue != null
                && element4.objectReferenceValue != null);
                if (element.stringValue == string.Empty) GUI.backgroundColor = Color.red;
                element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                GUI.backgroundColor = color;
                element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                if (element3.objectReferenceValue == null) GUI.backgroundColor = Color.red;
                element3.objectReferenceValue = EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.backgroundColor = color;
                if (element4.objectReferenceValue == null) GUI.backgroundColor = Color.red;
                element4.objectReferenceValue = EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.backgroundColor = color;
                allRight.BooleanEditorIcon();
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(1);
            EditorGUILayout.BeginHorizontal();
            if (_spoilersNames.arraySize == 0 || GUILayout.Button("+", GUILayout.MaxWidth(50)))
            {
                _spoilersNames.arraySize++;
                _spoilersCost.arraySize++;
                _spoilersSprites.arraySize++;
                _spoilersIconsSprites.arraySize++;
                _spoilersNames.GetArrayElementAtIndex(_spoilersNames.arraySize - 1).stringValue = string.Empty;
                _spoilersCost.GetArrayElementAtIndex(_spoilersCost.arraySize - 1).intValue = 0;
                _spoilersSprites.GetArrayElementAtIndex(_spoilersSprites.arraySize - 1).objectReferenceValue = null;
                _spoilersIconsSprites.GetArrayElementAtIndex(_spoilersIconsSprites.arraySize - 1).objectReferenceValue = null;
            }
            GUI.enabled = _spoilersNames.arraySize > 1;
            if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
            {
                _spoilersNames.arraySize--;
                _spoilersCost.arraySize--;
                _spoilersSprites.arraySize--;
                _spoilersIconsSprites.arraySize--;
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);
    }
    private void DrawSplitters()
    {
        EditorGUILayout.BeginHorizontal();
        _showSplitters = showSplitters.target = EditorGUILayout.BeginFoldoutHeaderGroup(_showSplitters, "Splitter");
        Color color = GUI.backgroundColor;
        SplitterDataIsSpecified().BooleanEditorIcon();
        EditorGUILayout.EndHorizontal();
        if (EditorGUILayout.BeginFadeGroup(showSplitters.faded))
        {
            for (int i = 0; i < _splittersNames.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty element = _splittersNames.GetArrayElementAtIndex(i);
                SerializedProperty element2 = _splittersCost.GetArrayElementAtIndex(i);
                SerializedProperty element3 = _splittersSprites.GetArrayElementAtIndex(i);
                SerializedProperty element4 = _splittersIconsSprites.GetArrayElementAtIndex(i);
                bool allRight = (element.stringValue != string.Empty
                && element3.objectReferenceValue != null
                && element4.objectReferenceValue != null);
                if (element.stringValue == string.Empty) GUI.backgroundColor = Color.red;
                element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                GUI.backgroundColor = color;
                element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                if (element3.objectReferenceValue == null) GUI.backgroundColor = Color.red;
                element3.objectReferenceValue = EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.backgroundColor = color;
                if (element4.objectReferenceValue == null) GUI.backgroundColor = Color.red;
                element4.objectReferenceValue = EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                GUILayout.Height(EditorGUIUtility.singleLineHeight));
                GUI.backgroundColor = color;
                allRight.BooleanEditorIcon();
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(1);
            EditorGUILayout.BeginHorizontal();
            if (_splittersNames.arraySize == 0 || GUILayout.Button("+", GUILayout.MaxWidth(50)))
            {
                _splittersNames.arraySize++;
                _splittersCost.arraySize++;
                _splittersSprites.arraySize++;
                _splittersIconsSprites.arraySize++;
                _splittersNames.GetArrayElementAtIndex(_splittersNames.arraySize - 1).stringValue = string.Empty;
                _splittersCost.GetArrayElementAtIndex(_splittersCost.arraySize - 1).intValue = 0;
                _splittersSprites.GetArrayElementAtIndex(_splittersSprites.arraySize - 1).objectReferenceValue = null;
                _splittersIconsSprites.GetArrayElementAtIndex(_splittersIconsSprites.arraySize - 1).objectReferenceValue = null;
            }
            GUI.enabled = _splittersNames.arraySize > 1;
            if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
            {
                _splittersNames.arraySize--;
                _splittersCost.arraySize--;
                _splittersSprites.arraySize--;
                _splittersIconsSprites.arraySize--;
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space(10);
    }
}
