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
        SerializedProperty _defaultSuspensionFrequency;
        SerializedProperty _defaultSuspensionHeight;

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
            get { return bool.Parse(EditorPrefs.GetString("tires", "false")); }
            set { EditorPrefs.SetString("tires", value.ToString()); }
        }
        private bool _showRims
        {
            get { return bool.Parse(EditorPrefs.GetString("rims", "false")); }
            set { EditorPrefs.SetString("rims", value.ToString()); }
        }
        private bool _showSpoilers
        {
            get { return bool.Parse(EditorPrefs.GetString("spoilers", "false")); }
            set { EditorPrefs.SetString("spoilers", value.ToString()); }
        }
        private bool _showSplitters
        {
            get { return bool.Parse(EditorPrefs.GetString("splitters", "false")); }
            set { EditorPrefs.SetString("splitters", value.ToString()); }
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
            _defaultSuspensionFrequency = serializedObject.FindProperty(nameof(_defaultSuspensionFrequency));
            _defaultSuspensionHeight = serializedObject.FindProperty(nameof(_defaultSuspensionHeight));
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

            serializedObject.ApplyModifiedProperties();
        }

        private void GenerateGuid()
        { _id.stringValue = System.Guid.NewGuid().ToString(); }
        private void DrawVariables()
        {
            EditorGUILayout.PropertyField(_acceleration);
            EditorGUILayout.PropertyField(_maxSpeed);
            EditorGUILayout.PropertyField(_maxBackSpeed);
            EditorGUILayout.PropertyField(_brakeForce);
            EditorGUILayout.PropertyField(_airBrakeForce);
            EditorGUILayout.PropertyField(_gearBrakeForce);
            EditorGUILayout.PropertyField(_defaultSuspensionFrequency);
            EditorGUILayout.PropertyField(_defaultSuspensionHeight);
            EditorGUILayout.PropertyField(_wheelSize);
        }
        private void DrawGear()
        {
            EditorGUILayout.PropertyField(_gearType);
            EditorGUILayout.PropertyField(_gearsMaxSpeed);
            EditorGUILayout.PropertyField(_maximumMotorForces);
        }
        private void DrawData()
        {
            EditorGUILayout.PropertyField(_baseSprite);
            EditorGUILayout.PropertyField(_backSprite);
            EditorGUILayout.PropertyField(_elementsSprite);
            EditorGUILayout.PropertyField(_opticsSprite);
            EditorGUILayout.Space(10);
            DrawTires();
            DrawRims();
            DrawSpoilers();
            DrawSplitters();
        }
        private void DrawInfo()
        {
            EditorGUILayout.PropertyField(_name);
            EditorGUILayout.PropertyField(_cost);
            EditorGUILayout.PropertyField(_weight);
            EditorGUILayout.PropertyField(_strength);
        }
        private void DrawTires()
        {
            _showTires = EditorGUILayout.BeginFoldoutHeaderGroup(_showTires, "Tires");
            if (_showTires)
            {
                for (int i = 0; i < _tiresNames.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty element = _tiresNames.GetArrayElementAtIndex(i);
                    element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                    SerializedProperty element2 = _tiresCost.GetArrayElementAtIndex(i);
                    element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                    SerializedProperty element3 = _tiresSprites.GetArrayElementAtIndex(i);
                    element3.objectReferenceValue =
                    EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    SerializedProperty element4 = _tiresIconsSprites.GetArrayElementAtIndex(i);
                    element4.objectReferenceValue =
                    EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(1);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                {
                    _tiresNames.arraySize++;
                    _tiresCost.arraySize++;
                    _tiresSprites.arraySize++;
                    _tiresIconsSprites.arraySize++;
                }
                if (_tiresNames.arraySize > 0)
                    if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                    {
                        _tiresNames.arraySize--;
                        _tiresCost.arraySize--;
                        _tiresSprites.arraySize--;
                        _tiresIconsSprites.arraySize--;
                    }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        private void DrawRims()
        {
            _showRims = EditorGUILayout.BeginFoldoutHeaderGroup(_showRims, "Rims");
            if (_showRims)
            {
                for (int i = 0; i < _rimsNames.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty element = _rimsNames.GetArrayElementAtIndex(i);
                    element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                    SerializedProperty element2 = _rimsCost.GetArrayElementAtIndex(i);
                    element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                    SerializedProperty element3 = _rimsSprites.GetArrayElementAtIndex(i);
                    element3.objectReferenceValue =
                    EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    SerializedProperty element4 = _rimsIconsSprites.GetArrayElementAtIndex(i);
                    element4.objectReferenceValue =
                    EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(1);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                {
                    _rimsNames.arraySize++;
                    _rimsCost.arraySize++;
                    _rimsSprites.arraySize++;
                    _rimsIconsSprites.arraySize++;
                }
                if (_rimsNames.arraySize > 0)
                    if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                    {
                        _rimsNames.arraySize--;
                        _rimsCost.arraySize--;
                        _rimsSprites.arraySize--;
                        _rimsIconsSprites.arraySize--;
                    }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        private void DrawSpoilers()
        {
            _showSpoilers = EditorGUILayout.BeginFoldoutHeaderGroup(_showSpoilers, "Spoilers");
            if (_showSpoilers)
            {
                for (int i = 0; i < _spoilersNames.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty element = _spoilersNames.GetArrayElementAtIndex(i);
                    element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                    SerializedProperty element2 = _spoilersCost.GetArrayElementAtIndex(i);
                    element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                    SerializedProperty element3 = _spoilersSprites.GetArrayElementAtIndex(i);
                    element3.objectReferenceValue =
                    EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    SerializedProperty element4 = _spoilersIconsSprites.GetArrayElementAtIndex(i);
                    element4.objectReferenceValue =
                    EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(1);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                {
                    _spoilersNames.arraySize++;
                    _spoilersCost.arraySize++;
                    _spoilersSprites.arraySize++;
                    _spoilersIconsSprites.arraySize++;
                }
                if (_spoilersNames.arraySize > 0)
                    if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                    {
                        _spoilersNames.arraySize--;
                        _spoilersCost.arraySize--;
                        _spoilersSprites.arraySize--;
                        _spoilersIconsSprites.arraySize--;
                    }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        private void DrawSplitters()
        {
            _showSplitters = EditorGUILayout.BeginFoldoutHeaderGroup(_showSplitters, "Splitter");
            if (_showSplitters)
            {
                for (int i = 0; i < _splittersNames.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty element = _splittersNames.GetArrayElementAtIndex(i);
                    element.stringValue = EditorGUILayout.TextField(element.stringValue, GUILayout.MaxWidth(maxWidth));
                    SerializedProperty element2 = _splittersCost.GetArrayElementAtIndex(i);
                    element2.intValue = EditorGUILayout.IntField(element2.intValue, GUILayout.MaxWidth(maxWidth));
                    SerializedProperty element3 = _splittersSprites.GetArrayElementAtIndex(i);
                    element3.objectReferenceValue =
                    EditorGUILayout.ObjectField(element3.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    SerializedProperty element4 = _splittersIconsSprites.GetArrayElementAtIndex(i);
                    element4.objectReferenceValue =
                    EditorGUILayout.ObjectField(element4.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space(1);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                {
                    _splittersNames.arraySize++;
                    _splittersCost.arraySize++;
                    _splittersSprites.arraySize++;
                    _splittersIconsSprites.arraySize++;
                }
                if (_splittersNames.arraySize > 0)
                    if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                    {
                        _splittersNames.arraySize--;
                        _splittersCost.arraySize--;
                        _splittersSprites.arraySize--;
                        _splittersIconsSprites.arraySize--;
                    }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
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
    [SerializeField] private float _defaultSuspensionFrequency = 4f;
    [SerializeField] private float _defaultSuspensionHeight = -0.219f;

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

    public float DefaultSuspensionFrequency => this._defaultSuspensionFrequency;
    public float DefaultSuspensionHeight => this._defaultSuspensionHeight;

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