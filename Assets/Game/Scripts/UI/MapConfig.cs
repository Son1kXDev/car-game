using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Game/New map config")]
public class MapConfig : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
    [SerializeField] private string _lenghts;
    [SerializeField] private FlatnessType _flatness;
    [SerializeField, TextArea] private string _description;

    public int SceneID => this._id;
    public string Name => this._name;
    public Sprite Image => this._image;
    public string Lenght => this._lenghts;
    public FlatnessType Flatness => this._flatness;
    public string Description => this._description;
}

public enum FlatnessType
{
    awful, bad, normal, good, flat
}