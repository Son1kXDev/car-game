using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Car", menuName = "Game/New car config")]
public class Car : ScriptableObject
{
    [SerializeField] private string _id;

    [ContextMenu("Generate guid ID")]
    private void GenerateGuid()
    { _id = System.Guid.NewGuid().ToString(); }

    [Header("Variables")]
    [SerializeField] private float _acceleration = 500f;
    [SerializeField] private float _maxSpeed = 800f;
    [SerializeField] private float _maxBackSpeed = 600f;
    [SerializeField] private float _brakeForce = 1000f;
    [SerializeField] private float _airBrakeForce = 500f;
    [SerializeField] private float _gearBrakeForce = 80f;
    [SerializeField] private float _wheelSize = 0.03f;

    [Header("Gear")]
    [SerializeField] private GearType _gearType = GearType.Full;
    [SerializeField] private List<int> _gearsMaxSpeed = new List<int> { 400, 800, 1200, 1500, 2000, 2200 };
    [SerializeField] private List<float> _maximumMotorForces = new List<float> { 2.5f, 2.25f, 2f, 1.85f, 1.5f, 1.25f };

    public string ID => this._id;
    public float Acceleration => this._acceleration;
    public float MaxSpeed => this._maxSpeed;
    public float MaxBackSpeed => this._maxBackSpeed;
    public float BrakeForce => this._brakeForce;
    public float AirBrakeForce => this._airBrakeForce;
    public float GearBrakeForce => this._gearBrakeForce;
    public float WheelSize => this._wheelSize;

    public GearType GearType => this._gearType;
    public List<int> GearsMaxSpeed => this._gearsMaxSpeed;
    public List<float> MaximumMotorForces => this._maximumMotorForces;
}

[System.Serializable]
public class Upgrades
{
    public float AccelirationMultiplier;
    public float MaxSpeedMultiplier;
    public float BreakForceMultiplier;
    public float GearSwitchMultiplier;
    public float SuspensionFrequencyMultiplier;
    public float SuspensionHeightMultiplier;

    public Upgrades()
    {
        AccelirationMultiplier = 1;
        MaxSpeedMultiplier = 1;
        BreakForceMultiplier = 1;
        GearSwitchMultiplier = 1;
        SuspensionFrequencyMultiplier = 1;
        SuspensionHeightMultiplier = 1;
    }

    public Upgrades(float acceliration, float maxSpeed, float breakForce, float gearSwitch, float suspensionFrequency, float suspensionHeight)
    {
        AccelirationMultiplier = acceliration;
        MaxSpeedMultiplier = maxSpeed;
        BreakForceMultiplier = breakForce;
        GearSwitchMultiplier = gearSwitch;
        SuspensionFrequencyMultiplier = suspensionFrequency;
        SuspensionHeightMultiplier = suspensionHeight;
    }
}