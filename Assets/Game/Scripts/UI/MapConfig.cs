using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "MapConfig", menuName = "Resources/Game/Map")]
public class MapConfig : ScriptableObject
{
    [SerializeField] private int _id = 0;
    [SerializeField] private string _name = "";
    [SerializeField] private Sprite _image;
    [SerializeField] private MapLength _length;
    [SerializeField] private FlatnessType _flatness;
    [SerializeField] private Description _description;
    [SerializeField] private DescriptionLang _lang;

    public int SceneID => this._id;
    public string Name => this._name;
    public Sprite Image => this._image;
    public MapLength Length => this._length;
    public FlatnessType Flatness => this._flatness;
    public Description Description => this._description;

    #region EDITOR
#if UNITY_EDITOR
#pragma warning disable CS0414
    [SerializeField] private SceneAsset _scene;
#pragma warning restore CS0414
    [ContextMenu("Reset Scene")]
    private void ResetScene()
    {
        _scene = null;
        _id = 0;
    }
    [CustomEditor(typeof(MapConfig))]
    [System.Obsolete]
    public class MapConfigEditor : Editor
    {
        private SerializedProperty _id;
        private SerializedProperty _scene;
        private SerializedProperty _name;
        private SerializedProperty _image;
        private SerializedProperty _length;
        private SerializedProperty _flatness;
        private SerializedProperty _description;

        private void OnEnable()
        {
            _id = serializedObject.FindProperty(nameof(_id));
            _scene = serializedObject.FindProperty(nameof(_scene));
            _name = serializedObject.FindProperty(nameof(_name));
            _image = serializedObject.FindProperty(nameof(_image));
            _length = serializedObject.FindProperty(nameof(_length));
            _flatness = serializedObject.FindProperty(nameof(_flatness));
            _description = serializedObject.FindProperty(nameof(_description));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_scene.objectReferenceValue == null)
            {
                EditorGUILayout.PropertyField(_scene);
                EditorGUILayout.HelpBox("Please specify a scene reference", MessageType.Warning);
                serializedObject.ApplyModifiedProperties();
                return;
            }
            if (SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(_scene.objectReferenceValue)) < 3)
            {
                _scene.objectReferenceValue = null;
                _id.intValue = 0;
            }
            EditorGUILayout.Space(10);
            _id.intValue = SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(_scene.objectReferenceValue));
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 75f;
            EditorGUILayout.BeginVertical();
            _image.objectReferenceValue =
            EditorGUILayout.ObjectField(_image.objectReferenceValue, typeof(Sprite), GUILayout.Width(168), GUILayout.MinHeight(72));
            if (_image.objectReferenceValue == null)
                EditorGUILayout.HelpBox("Sprite preview reference is null", MessageType.Warning);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            _name.stringValue = EditorGUILayout.TextField(_name.stringValue, GUILayout.MinWidth(10));
            if (_name.stringValue == string.Empty)
                EditorGUILayout.HelpBox("Map name is empty", MessageType.Warning);
            EditorGUILayout.PropertyField(_length);
            EditorGUILayout.PropertyField(_flatness);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);

            MapConfig config = (MapConfig)target;
            config._lang = (DescriptionLang)EditorGUILayout.EnumPopup("Description", config._lang, GUILayout.MaxWidth(120));
            if (config._lang == DescriptionLang.EN)
                config._description.EN =
                EditorGUILayout.TextArea(config._description.EN, EditorStyles.textArea, GUILayout.MinHeight(40), GUILayout.MaxHeight(100));
            else config._description.RU =
            EditorGUILayout.TextArea(config._description.RU, EditorStyles.textArea, GUILayout.MinHeight(40), GUILayout.MaxHeight(100));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion

}

[System.Serializable]
public class Description
{
    [TextArea] public string EN;
    [TextArea] public string RU;
}

public enum DescriptionLang { EN, RU };

public enum FlatnessType { awful, bad, normal, good, flat }

public enum FlatnessTypeRU { ужасная, плохая, нормальная, хорошая, плоская }

public enum MapLength { tiny, small, medium, big, large, humongous }

public enum MapLengthRU { крошечная, маленькая, средняя, большая, крупная, огромная }