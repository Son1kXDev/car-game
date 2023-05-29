using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Car Visual", menuName = "Game/New car visual config")]
public class CarVisualConfig : ScriptableObject
{

    [Header("Sprites")]
    [SerializeField] private Sprite _baseSprite;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Sprite _elementsSprite;
    [SerializeField] private Sprite _opticsSprite;
    [SerializeField] private List<Sprite> _tiresSprites;
    [SerializeField] private List<Sprite> _rimsSprites;
    [SerializeField] private List<Sprite> _spoilersSprites;
    [SerializeField] private List<Sprite> _splittersSprites;

    [Header("Icons")]
    [SerializeField] private List<Sprite> _tiresIconsSprites;
    [SerializeField] private List<Sprite> _rimsIconsSprites;
    [SerializeField] private List<Sprite> _spoilersIconsSprites;
    [SerializeField] private List<Sprite> _splittersIconsSprites;

    [Header("Info")]
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

    public Sprite BaseSprite => _baseSprite;
    public Sprite BackSprite => _backSprite;
    public Sprite ElementsSprite => _elementsSprite;
    public Sprite OpticsSprite => _opticsSprite;
    public List<Sprite> TiresSprites => _tiresSprites;
    public List<Sprite> RimsSprites => _rimsSprites;
    public List<Sprite> SpoilersSprites => _spoilersSprites;
    public List<Sprite> SplittersSprites => _splittersSprites;
    public List<Sprite> TiresIconsSprites => _tiresIconsSprites;
    public List<Sprite> RimsIconsSprites => _rimsIconsSprites;
    public List<Sprite> SpoilersIconsSprites => _spoilersIconsSprites;
    public List<Sprite> SplittersIconsSprites => _splittersIconsSprites;

    public List<string> TiresNames => _tiresNames;
    public List<string> RimsNames => _rimsNames;
    public List<string> SpoilersNames => _spoilersNames;
    public List<string> SplittersNames => _splittersNames;
    public List<int> TiresCost => _tiresCost;
    public List<int> RimsCost => _rimsCost;
    public List<int> SpoilersCost => _spoilersCost;
    public List<int> SplittersCost => _splittersCost;

    public string Name => _name;
    public float Cost => _cost;
    public float Weight => _weight;
    public float Strength => _strength;
}