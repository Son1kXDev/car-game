using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Car Visual", menuName = "Game/New car visual config")]
public class CarVisualConfig : ScriptableObject
{
    [SerializeField, Tooltip("Copy ID from car main config")] private string _id;

    [Header("Sprites")]
    [SerializeField] private Sprite _baseSprite;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Sprite _elementsSprite;
    [SerializeField] private Sprite _opticsSprite;
    [SerializeField] private List<Sprite> _tiresSprites;
    [SerializeField] private List<Sprite> _rimsSprites;

    [Header("Info")]
    [SerializeField] private string _name;
    [SerializeField] private float _cost;
    [SerializeField] private float _weight;
    [SerializeField] private float _strength;
    [SerializeField] private List<string> _tiresNames;
    [SerializeField] private List<int> _tiresCost;
    [SerializeField] private List<string> _rimsNames;
    [SerializeField] private List<int> _rimsCost;

    public string ID => this._id;
    public Sprite BaseSprite => _baseSprite;
    public Sprite BackSprite => _backSprite;
    public Sprite ElementsSprite => _elementsSprite;
    public Sprite OpticsSprite => _opticsSprite;
    public List<Sprite> TiresSprites => _tiresSprites;
    public List<Sprite> RimsSprites => _rimsSprites;

    public List<string> TiresNames => _tiresNames;
    public List<string> RimsNames => _rimsNames;
    public List<int> TiresCost => _tiresCost;
    public List<int> RimsCost => _rimsCost;

    public string Name => _name;
    public float Cost => _cost;
    public float Weight => _weight;
    public float Strength => _strength;
}