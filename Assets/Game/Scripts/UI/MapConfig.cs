using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Game/New map config")]
public class MapConfig : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
    [SerializeField] private MapLength _length;
    [SerializeField] private FlatnessType _flatness;
    [SerializeField] private Description _description;

    public int SceneID => this._id;
    public string Name => this._name;
    public Sprite Image => this._image;
    public MapLength Length => this._length;
    public FlatnessType Flatness => this._flatness;
    public Description Description => this._description;

}

[System.Serializable]
public class Description
{
    [TextArea] public string EN;
    [TextArea] public string RU;
}

public enum FlatnessType { awful, bad, normal, good, flat }

public enum FlatnessTypeRU { ужасная, плохая, нормальная, хорошая, плоская }

public enum MapLength { tiny, small, medium, big, large, humongous }

public enum MapLengthRU { крошечная, маленькая, средняя, большая, крупная, огромная }