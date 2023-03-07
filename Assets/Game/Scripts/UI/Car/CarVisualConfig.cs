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
    [SerializeField] private List<Sprite> _wheelsSprites;

    [Header("Info")]
    [SerializeField] private float _cost;
    [SerializeField] private float _weight;
    [SerializeField] private float _strength;

    public string ID => this._id;
    public Sprite BaseSprite => _baseSprite;
    public Sprite BackSprite => _backSprite;
    public Sprite ElementsSprite => _elementsSprite;
    public Sprite OpticsSprite => _opticsSprite;
    public List<Sprite> WheelsSprites => _wheelsSprites;

    public float Cost => _cost;
    public float Weight => _weight;
    public float Strength => _strength;
}