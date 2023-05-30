using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Car", menuName = "Resources/Game/Car", order = 0)]
public class Car : ScriptableObject
{
    [SerializeField] private string _id;

    [ContextMenu("Generate guid ID")]
    private void GenerateGuid()
    { _id = System.Guid.NewGuid().ToString(); }

    [ContextMenu("Reset ID")]
    private void ResetID()
    { _id = string.Empty; }

    #region EDITOR
#if UNITY_EDITOR
    private enum SerializedPropertyType { Variables, Gear, Data, Information }
    [SerializeField] private SerializedPropertyType _property;

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
            EditorGUILayout.PropertyField(_property);
            EditorGUILayout.Space(10);

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

            if (AllDataIsSpecified() == false)
                HelpBox("Some variables are not specified", MessageType.Error);

            serializedObject.ApplyModifiedProperties();
        }
        private void GenerateGuid()
        { _id.stringValue = System.Guid.NewGuid().ToString(); }
        private void DrawVariables()
        {
            _accelerationHelpBox = PropertyField(_acceleration, _accelerationHelpBox, "Wheel acceleration speed.");
            _maxSpeedHelpBox = PropertyField(_maxSpeed, _maxSpeedHelpBox,
            "The maximum allowable wheel speed. \nDo nothing now, because of Gear / Gears Max Speed");
            _maxBackSpeedHelpBox = PropertyField(_maxBackSpeed, _maxBackSpeedHelpBox, "The maximum allowable reverse wheel speed.");
            _brakeForceHelpBox = PropertyField(_brakeForce, _brakeForceHelpBox, "Deceleration speed when the brake button is pressed.");
            _airBrakeForceHelpBox = PropertyField(_airBrakeForce, _airBrakeForceHelpBox, "Deceleration speed due to air friction.");
            _gearBrakeForceHelpBox = PropertyField(_gearBrakeForce, _gearBrakeForceHelpBox, "Deceleration speed when changing gear.");
            _suspensionFrequencyHelpBox = PropertyField(_suspensionFrequency, _suspensionFrequencyHelpBox,
            "Default value for the car's suspension frequency.");
            _suspensionHeightHelpBox = PropertyField(_suspensionHeight, _suspensionHeightHelpBox,
            "Default value for the car's suspension height.");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("?", GUILayout.Width(EditorGUIUtility.singleLineHeight),
            GUILayout.Height(EditorGUIUtility.singleLineHeight))) _wheelSizeHelpBox = !_wheelSizeHelpBox;
            EditorGUILayout.PropertyField(_wheelSize);
            if (_wheelSize.floatValue < 0) _wheelSize.floatValue = 0;
            EditorGUILayout.EndHorizontal();

            if (_wheelSizeHelpBox)
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                string helpBox = "Assign this config to your prefab's CarConfig.cs script to fine-tune the Wheel Size value.";
                helpBox += "\nIf you already have this config set to your Car prefab, simply select your Car object in inspector";
                helpBox += " and it will automatically show your gizmos for Wheel Size value.";
                Texture2D texture = (Texture2D)EditorGUIUtility.Load("Assets/Resources/Editor/WheelSizePreview.png");
                EditorGUILayout.LabelField("", EditorStyles.helpBox, GUILayout.Height(64), GUILayout.Width(64));
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
                HelpBox(helpBox, MessageType.None);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void HelpBox(string message, MessageType type = MessageType.Info)
        { EditorGUILayout.HelpBox(message, type); }

        private void PropertyField(SerializedProperty property, Warning warning = Warning.Warning)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(property);
            string icon = warning == Warning.Warning ? "console.warnicon.sml" : "CollabError";
            if (warning != Warning.None)
                EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.IconContent(icon), GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();
        }

        private enum Warning { None = -1, Warning = 1, Error = 2 }

        private bool PropertyField(SerializedProperty property, bool button, string helpMessage)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("?", GUILayout.Width(EditorGUIUtility.singleLineHeight),
            GUILayout.Height(EditorGUIUtility.singleLineHeight))) button = !button;
            EditorGUILayout.PropertyField(property);
            EditorGUILayout.EndHorizontal();
            if (button) HelpBox(helpMessage, MessageType.None);
            return button;
        }

        private void DrawGear()
        {
            PropertyField(_gearType);
            PropertyField(_gearsMaxSpeed);
            PropertyField(_maximumMotorForces);
            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("These parameters are deprecated and will be removed in the future.", MessageType.Warning);

        }

        private void DrawData()
        {
            PropertyField(_baseSprite, _baseSprite.objectReferenceValue == null ? Warning.Error : Warning.None);
            PropertyField(_backSprite, _backSprite.objectReferenceValue == null ? Warning.Error : Warning.None);
            PropertyField(_elementsSprite, _elementsSprite.objectReferenceValue == null ? Warning.Error : Warning.None);
            PropertyField(_opticsSprite, _opticsSprite.objectReferenceValue == null ? Warning.Error : Warning.None);
            EditorGUILayout.Space(10);
            DrawTires();
            DrawRims();
            DrawSpoilers();
            DrawSplitters();

            EditorGUILayout.Space(10);
        }

        private bool AllDataIsSpecified()
        {
            return _baseSprite.objectReferenceValue != null && _backSprite.objectReferenceValue != null &&
                        _elementsSprite.objectReferenceValue != null && _opticsSprite.objectReferenceValue != null &&
                        TiresDataIsSpecified() && RimsDataIsSpecified() && SpoilersDataIsSpecified() && SplitterDataIsSpecified() &&
                        _name.stringValue != string.Empty && _cost.floatValue != 0 && _weight.floatValue != 0 && _strength.floatValue != 0;
        }

        private bool TiresDataIsSpecified()
        {
            for (int i = 0; i < _tiresNames.arraySize; i++)
            {
                SerializedProperty element = _tiresNames.GetArrayElementAtIndex(i);
                SerializedProperty element3 = _tiresSprites.GetArrayElementAtIndex(i);
                SerializedProperty element4 = _tiresIconsSprites.GetArrayElementAtIndex(i);

                if (element.stringValue == string.Empty) return false;
                if (element3.objectReferenceValue == null) return false;
                if (element4.objectReferenceValue == null) return false;
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

                if (element.stringValue == string.Empty) return false;
                if (element3.objectReferenceValue == null) return false;
                if (element4.objectReferenceValue == null) return false;
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

                if (element.stringValue == string.Empty) return false;
                if (element3.objectReferenceValue == null) return false;
                if (element4.objectReferenceValue == null) return false;
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

                if (element.stringValue == string.Empty) return false;
                if (element3.objectReferenceValue == null) return false;
                if (element4.objectReferenceValue == null) return false;
            }
            return _splittersNames.arraySize > 0;
        }

        private void DrawInfo()
        {
            PropertyField(_name, _name.stringValue == string.Empty ? Warning.Error : Warning.None);
            PropertyField(_cost, _cost.floatValue == 0 ? Warning.Error : Warning.None);
            PropertyField(_weight, _weight.floatValue == 0 ? Warning.Error : Warning.None);
            PropertyField(_strength, _strength.floatValue == 0 ? Warning.Error : Warning.None);

            EditorGUILayout.Space(10);
        }

        private void ErrorIcon()
        { EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.IconContent("CollabError"), GUILayout.Width(20)); }

        private void DrawTires()
        {
            EditorGUILayout.BeginHorizontal();
            _showTires = EditorGUILayout.BeginFoldoutHeaderGroup(_showTires, "Tires");
            if (!TiresDataIsSpecified())
                ErrorIcon();
            EditorGUILayout.EndHorizontal();
            if (_showTires)
            {
                for (int i = 0; i < _tiresNames.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty element = _tiresNames.GetArrayElementAtIndex(i);
                    SerializedProperty element2 = _tiresCost.GetArrayElementAtIndex(i);
                    SerializedProperty element3 = _tiresSprites.GetArrayElementAtIndex(i);
                    SerializedProperty element4 = _tiresIconsSprites.GetArrayElementAtIndex(i);
                    element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                    element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                    element3.objectReferenceValue =
                    EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    element4.objectReferenceValue =
                    EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    if (element.stringValue == string.Empty || element3.objectReferenceValue == null || element4.objectReferenceValue == null)
                        ErrorIcon();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(1);
                EditorGUILayout.BeginHorizontal();
                if (_tiresNames.arraySize == 0)
                    ErrorIcon();
                if (GUILayout.Button(_tiresNames.arraySize != 0 ? "+" : "Create", GUILayout.MaxWidth(50)))
                {
                    _tiresNames.arraySize++;
                    _tiresCost.arraySize++;
                    _tiresSprites.arraySize++;
                    _tiresIconsSprites.arraySize++;
                }
                if (_tiresNames.arraySize > 0)
                {
                    GUI.enabled = _tiresNames.arraySize > 1;
                    if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                    {
                        _tiresNames.arraySize--;
                        _tiresCost.arraySize--;
                        _tiresSprites.arraySize--;
                        _tiresIconsSprites.arraySize--;
                    }
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(10);
        }
        private void DrawRims()
        {
            EditorGUILayout.BeginHorizontal();
            _showRims = EditorGUILayout.BeginFoldoutHeaderGroup(_showRims, "Rims");
            if (!RimsDataIsSpecified())
                ErrorIcon();
            EditorGUILayout.EndHorizontal();
            if (_showRims)
            {
                for (int i = 0; i < _rimsNames.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty element = _rimsNames.GetArrayElementAtIndex(i);
                    SerializedProperty element2 = _rimsCost.GetArrayElementAtIndex(i);
                    SerializedProperty element3 = _rimsSprites.GetArrayElementAtIndex(i);
                    SerializedProperty element4 = _rimsIconsSprites.GetArrayElementAtIndex(i);
                    element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                    element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                    element3.objectReferenceValue =
                    EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    element4.objectReferenceValue =
                    EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    if (element.stringValue == string.Empty || element3.objectReferenceValue == null || element4.objectReferenceValue == null)
                        ErrorIcon();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(1);
                EditorGUILayout.BeginHorizontal();
                if (_rimsNames.arraySize == 0)
                    ErrorIcon();
                if (GUILayout.Button(_rimsNames.arraySize != 0 ? "+" : "Create", GUILayout.MaxWidth(50)))
                {
                    _rimsNames.arraySize++;
                    _rimsCost.arraySize++;
                    _rimsSprites.arraySize++;
                    _rimsIconsSprites.arraySize++;
                }
                if (_rimsNames.arraySize > 0)
                {
                    GUI.enabled = _rimsNames.arraySize > 1;
                    if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                    {
                        _rimsNames.arraySize--;
                        _rimsCost.arraySize--;
                        _rimsSprites.arraySize--;
                        _rimsIconsSprites.arraySize--;
                    }
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(10);
        }
        private void DrawSpoilers()
        {
            EditorGUILayout.BeginHorizontal();
            _showSpoilers = EditorGUILayout.BeginFoldoutHeaderGroup(_showSpoilers, "Spoilers");
            if (!SpoilersDataIsSpecified())
                ErrorIcon();
            EditorGUILayout.EndHorizontal();
            if (_showSpoilers)
            {
                for (int i = 0; i < _spoilersNames.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty element = _spoilersNames.GetArrayElementAtIndex(i);
                    SerializedProperty element2 = _spoilersCost.GetArrayElementAtIndex(i);
                    SerializedProperty element3 = _spoilersSprites.GetArrayElementAtIndex(i);
                    SerializedProperty element4 = _spoilersIconsSprites.GetArrayElementAtIndex(i);
                    element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                    element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                    element3.objectReferenceValue =
                    EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    element4.objectReferenceValue =
                    EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    if (element.stringValue == string.Empty || element3.objectReferenceValue == null || element4.objectReferenceValue == null)
                        ErrorIcon();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(1);
                EditorGUILayout.BeginHorizontal();
                if (_spoilersNames.arraySize == 0)
                    ErrorIcon();
                if (GUILayout.Button(_spoilersNames.arraySize != 0 ? "+" : "Create", GUILayout.MaxWidth(50)))
                {
                    _spoilersNames.arraySize++;
                    _spoilersCost.arraySize++;
                    _spoilersSprites.arraySize++;
                    _spoilersIconsSprites.arraySize++;
                }
                if (_spoilersNames.arraySize > 0)
                {
                    GUI.enabled = _spoilersNames.arraySize > 1;
                    if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                    {
                        _spoilersNames.arraySize--;
                        _spoilersCost.arraySize--;
                        _spoilersSprites.arraySize--;
                        _spoilersIconsSprites.arraySize--;
                    }
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(10);
        }
        private void DrawSplitters()
        {
            EditorGUILayout.BeginHorizontal();
            _showSplitters = EditorGUILayout.BeginFoldoutHeaderGroup(_showSplitters, "Splitter");
            if (!SplitterDataIsSpecified())
                ErrorIcon();
            EditorGUILayout.EndHorizontal();
            if (_showSplitters)
            {
                for (int i = 0; i < _splittersNames.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty element = _splittersNames.GetArrayElementAtIndex(i);
                    SerializedProperty element2 = _splittersCost.GetArrayElementAtIndex(i);
                    SerializedProperty element3 = _splittersSprites.GetArrayElementAtIndex(i);
                    SerializedProperty element4 = _splittersIconsSprites.GetArrayElementAtIndex(i);
                    element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                    element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                    element3.objectReferenceValue =
                    EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    element4.objectReferenceValue =
                    EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    if (element.stringValue == string.Empty || element3.objectReferenceValue == null || element4.objectReferenceValue == null)
                        ErrorIcon();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(1);
                EditorGUILayout.BeginHorizontal();
                if (_splittersNames.arraySize == 0)
                    ErrorIcon();
                if (GUILayout.Button(_splittersNames.arraySize != 0 ? "+" : "Create", GUILayout.MaxWidth(50)))
                {
                    _splittersNames.arraySize++;
                    _splittersCost.arraySize++;
                    _splittersSprites.arraySize++;
                    _splittersIconsSprites.arraySize++;
                }
                if (_splittersNames.arraySize > 0)
                {
                    GUI.enabled = _splittersNames.arraySize > 1;
                    if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                    {
                        _splittersNames.arraySize--;
                        _splittersCost.arraySize--;
                        _splittersSprites.arraySize--;
                        _splittersIconsSprites.arraySize--;
                    }
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(10);
        }
    }
#endif
    #endregion

    [SerializeField] private float _acceleration = 500f;
    [SerializeField] private float _maxSpeed = 800f;
    [SerializeField] private float _maxBackSpeed = 600f;
    [SerializeField] private float _brakeForce = 1000f;
    [SerializeField] private float _airBrakeForce = 500f;
    [SerializeField] private float _gearBrakeForce = 80f;
    [SerializeField] private float _wheelSize = 0.03f;
    [SerializeField] private float _suspensionFrequency = 4f;
    [SerializeField] private float _suspensionHeight = -0.219f;

    [SerializeField] private GearType _gearType = GearType.Full;
    [SerializeField] private List<int> _gearsMaxSpeed = new List<int> { 400, 800, 1200, 1500, 2000, 2200 };
    [SerializeField] private List<float> _maximumMotorForces = new List<float> { 2.5f, 2.25f, 2f, 1.85f, 1.5f, 1.25f };

    [SerializeField] private Sprite _baseSprite;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Sprite _elementsSprite;
    [SerializeField] private Sprite _opticsSprite;
    [SerializeField] private List<Sprite> _tiresSprites;
    [SerializeField] private List<Sprite> _rimsSprites;
    [SerializeField] private List<Sprite> _spoilersSprites;
    [SerializeField] private List<Sprite> _splittersSprites;
    [SerializeField] private List<Sprite> _tiresIconsSprites;
    [SerializeField] private List<Sprite> _rimsIconsSprites;
    [SerializeField] private List<Sprite> _spoilersIconsSprites;
    [SerializeField] private List<Sprite> _splittersIconsSprites;
    [SerializeField] private string _name;
    [SerializeField] private float _cost;
    [SerializeField] private float _weight;
    [SerializeField] private float _strength;
    [SerializeField] private List<string> _tiresNames;
    [SerializeField] private List<int> _tiresCost;
    [SerializeField] private List<string> _rimsNames;
    [SerializeField] private List<int> _rimsCost;
    [SerializeField] private List<string> _spoilersNames;
    [SerializeField] private List<int> _spoilersCost;
    [SerializeField] private List<string> _splittersNames;
    [SerializeField] private List<int> _splittersCost;

    public string ID => this._id;
    public float Acceleration => this._acceleration;
    public float MaxSpeed => this._maxSpeed;
    public float MaxBackSpeed => this._maxBackSpeed;
    public float BrakeForce => this._brakeForce;
    public float AirBrakeForce => this._airBrakeForce;
    public float GearBrakeForce => this._gearBrakeForce;
    public float WheelSize => this._wheelSize;

    public float SuspensionFrequency => this._suspensionFrequency;
    public float SuspensionHeight => this._suspensionHeight;

    public GearType GearType => this._gearType;
    public List<int> GearsMaxSpeed => this._gearsMaxSpeed;
    public List<float> MaximumMotorForces => this._maximumMotorForces;

    public Sprite BaseSprite => this._baseSprite;
    public Sprite BackSprite => this._backSprite;
    public Sprite ElementsSprite => this._elementsSprite;
    public Sprite OpticsSprite => this._opticsSprite;
    public List<Sprite> TiresSprites => this._tiresSprites;
    public List<Sprite> RimsSprites => this._rimsSprites;
    public List<Sprite> SpoilersSprites => this._spoilersSprites;
    public List<Sprite> SplittersSprites => this._splittersSprites;
    public List<Sprite> TiresIconsSprites => this._tiresIconsSprites;
    public List<Sprite> RimsIconsSprites => this._rimsIconsSprites;
    public List<Sprite> SpoilersIconsSprites => this._spoilersIconsSprites;
    public List<Sprite> SplittersIconsSprites => this._splittersIconsSprites;

    public string Name => _name;
    public float Cost => _cost;
    public float Weight => _weight;
    public float Strength => _strength;
    public List<string> TiresNames => _tiresNames;
    public List<string> RimsNames => _rimsNames;
    public List<string> SpoilersNames => _spoilersNames;
    public List<string> SplittersNames => _splittersNames;
    public List<int> TiresCost => _tiresCost;
    public List<int> RimsCost => _rimsCost;
    public List<int> SpoilersCost => _spoilersCost;
    public List<int> SplittersCost => _splittersCost;


}

[System.Serializable]
public class Upgrades
{
    public float EngineMultiplier;
    public float AccelerationMultiplier;
    public float MaxSpeedMultiplier;
    public float BreakForceMultiplier;
    public float GearSwitchMultiplier;
    public float SuspensionFrequencyMultiplier;
    public float SuspensionHeightMultiplier;

    public Upgrades()
    {
        EngineMultiplier = 1;
        AccelerationMultiplier = 1;
        MaxSpeedMultiplier = 1;
        BreakForceMultiplier = 1;
        GearSwitchMultiplier = 1;
        SuspensionFrequencyMultiplier = 1;
        SuspensionHeightMultiplier = 1;
    }

    public Upgrades(float engine, float acceleration, float maxSpeed, float breakForce, float gearSwitch, float suspensionFrequency, float suspensionHeight)
    {
        EngineMultiplier = engine;
        AccelerationMultiplier = acceleration;
        MaxSpeedMultiplier = maxSpeed;
        BreakForceMultiplier = breakForce;
        GearSwitchMultiplier = gearSwitch;
        SuspensionFrequencyMultiplier = suspensionFrequency;
        SuspensionHeightMultiplier = suspensionHeight;
    }
}