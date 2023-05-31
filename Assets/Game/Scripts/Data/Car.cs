using System.Collections.Generic;
using UnityEngine;

public enum SerializedPropertyType { Variables, Gear, Data, Information }
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

    [SerializeField] private SerializedPropertyType _property;
    [SerializeField, Min(0)] private float _acceleration = 500f;
    [SerializeField, Min(0)] private float _maxSpeed = 800f;
    [SerializeField, Min(0)] private float _maxBackSpeed = 600f;
    [SerializeField, Min(0)] private float _brakeForce = 1000f;
    [SerializeField, Min(0)] private float _airBrakeForce = 500f;
    [SerializeField, Min(0)] private float _gearBrakeForce = 80f;
    [SerializeField, Min(0)] private float _wheelSize = 0f;
    [SerializeField, Min(0)] private float _suspensionFrequency = 4f;
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
    [SerializeField, Min(0)] private float _cost;
    [SerializeField, Min(0)] private float _weight;
    [SerializeField, Min(0)] private float _strength;
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
