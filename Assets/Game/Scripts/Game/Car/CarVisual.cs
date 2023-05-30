using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Utils.Debugger;

#if UNITY_EDITOR
using UnityEditor;
using System;
#endif

namespace Assets.Game.Scripts.Game
{
    [RequireComponent(typeof(CarConfig))]
    public class CarVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _carBase;
        [SerializeField] private SpriteRenderer _carBack;
        [SerializeField] private SpriteRenderer _carElements;
        [SerializeField] private SpriteRenderer _carOptics;
        [SerializeField] private SpriteRenderer _carSticker;
        [SerializeField] private List<SpriteRenderer> _tires;
        [SerializeField] private List<SpriteRenderer> _rims;
        [SerializeField] private SpriteRenderer _spoiler;
        [SerializeField] private SpriteRenderer _splitter;
        [SerializeField] private GameObject _lowBeam;
        [SerializeField] private GameObject _highBeam;
        [SerializeField] private GameObject _backCasualLights;
        [SerializeField] private Animator _backMoveLights;
        [SerializeField] private Animator _brakeLights;
        [SerializeField] private EventReference _lightSwitchSound;

        #region EDITOR
#if UNITY_EDITOR
        private enum SerializedPropertyType { Sprites, Lights, Audio }
        [SerializeField] private SerializedPropertyType _property;

        [CustomEditor(typeof(CarVisual))]
        [Obsolete]
        public class CarVisualEditor : Editor
        {
            #region SerializedProperties
            SerializedProperty _property;
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

            private float maxWidth = 190;
            private void OnEnable()
            {
                _property = serializedObject.FindProperty(nameof(_property));
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
                serializedObject.Update();

                EditorGUILayout.PropertyField(_property);

                switch (Enum.GetValues(typeof(SerializedPropertyType)).GetValue(_property.enumValueIndex))
                {
                    case SerializedPropertyType.Sprites:
                        DrawSprites();
                        break;
                    case SerializedPropertyType.Lights:
                        DrawLights();
                        break;
                    case SerializedPropertyType.Audio:
                        DrawAudio();
                        break;
                }

                serializedObject.ApplyModifiedProperties();
            }

            private void DrawSprites()
            {
                if (AllDataIsSpecified() == false)
                    EditorGUILayout.HelpBox("Some variables are not specified", MessageType.Warning);
                EditorGUILayout.PropertyField(_carBase);
                EditorGUILayout.PropertyField(_carBack);
                EditorGUILayout.PropertyField(_carElements);
                EditorGUILayout.PropertyField(_carOptics);
                EditorGUILayout.PropertyField(_carSticker);
                DrawTires();
                DrawRims();
                EditorGUILayout.PropertyField(_spoiler);
                EditorGUILayout.PropertyField(_splitter);
            }
            private void DrawLights()
            {
                EditorGUILayout.PropertyField(_lowBeam);
                EditorGUILayout.PropertyField(_highBeam);
                EditorGUILayout.PropertyField(_backCasualLights);
                EditorGUILayout.PropertyField(_backMoveLights);
                EditorGUILayout.PropertyField(_brakeLights);
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
                for (int i = 0; i < _tires.arraySize; i++)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField(i == 0 ? "Back Tire" : "Front Tire");
                    SerializedProperty element = _tires.GetArrayElementAtIndex(i);
                    element.objectReferenceValue =
                    EditorGUILayout.ObjectField(element.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.MaxWidth(maxWidth));
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            private void DrawRims()
            {
                _rims.arraySize = 2;
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < _rims.arraySize; i++)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField(i == 0 ? "Back Rim" : "Front Rim");
                    SerializedProperty element = _rims.GetArrayElementAtIndex(i);
                    element.objectReferenceValue =
                    EditorGUILayout.ObjectField(element.objectReferenceValue, typeof(Sprite),
                    GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.MaxWidth(maxWidth));
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }

        }
#endif
        #endregion

        private CarConfig _config;
        private Beam _currentBeam = Beam.Disabled;
        private int _currentTire;
        private int _currentRim;

        private void Awake()
        {
            _lowBeam.SetActive(_currentBeam == Beam.Low);
            _highBeam.SetActive(_currentBeam == Beam.High);
            _backCasualLights.SetActive(true);
        }

        private void Start()
        {
            _config = GetComponent<CarConfig>();
            if (!_config.HasConfig())
            {
                Utils.Debugger.Console.
                LogError("Config files were not found. Please check if the files are set correctly or define them manually");
                return;
            }

            _carBase.sprite = _config.CurrentCar.BaseSprite;
            _carBack.sprite = _config.CurrentCar.BackSprite;
            _carElements.sprite = _config.CurrentCar.ElementsSprite;
            _carOptics.sprite = _config.CurrentCar.OpticsSprite;
            _carBase.color = _config.CurrentColor;
            _carSticker.sprite = StickerUploader.GetSprite(_config.CurrentStickerPath);
            _tires.ForEach(tire => tire.sprite = _config.CurrentCar.TiresSprites[_config.CurrentTire]);
            _rims.ForEach(rim => rim.sprite = _config.CurrentCar.RimsSprites[_config.CurrentRim]);
            _spoiler.sprite = _config.CurrentCar.SpoilersSprites[_config.CurrentSpoiler];
            _splitter.sprite = _config.CurrentCar.SplittersSprites[_config.CurrentSplitter];
        }

        public void SetLight(float axis, bool brake)
        {
            _backCasualLights.SetActive(_currentBeam != Beam.Disabled);
            _brakeLights.SetBool("active", brake);
            _backMoveLights.SetBool("active", !brake && axis < 0);
        }

        public void SwitchBeam(bool value)
        {
            switch (_currentBeam)
            {
                case Beam.Disabled:
                    _currentBeam = Beam.Low;
                    break;

                case Beam.Low:
                    _currentBeam = Beam.High;
                    break;

                case Beam.High:
                    _currentBeam = Beam.Disabled;
                    break;
            }
            _lowBeam.SetActive(_currentBeam == Beam.Low);
            _highBeam.SetActive(_currentBeam == Beam.High);
            AudioManager.Instance.PlayOneShot(Audio.Data.LightSwitch, transform.position);
        }

        public Beam GetCurrentBeam()
        { return _currentBeam; }
    }

    public enum Beam
    { Disabled, Low, High }
}