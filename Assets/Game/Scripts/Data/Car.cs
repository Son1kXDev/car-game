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
    [SerializeField] private float _forwardMoveForce = 800f;
    [SerializeField] private float _backMoveForce = 300f;
    [SerializeField] private float _brakeForce = 500f;
    [SerializeField] private float _airBrakeForce = 500f;
    [SerializeField] private float _gearBrakeForce = 80f;
    [SerializeField] private float _wheelSize = 0.03f;
    [SerializeField] private float _defaultSuspensionFrequency = 4f;
    [SerializeField] private float _defaultSuspensionHeight = -0.219f;

    [Header("Gear")]
    [SerializeField] private GearType _gearType = GearType.Full;

    public string ID => this._id;
    public float Acceleration => this._acceleration;
    public float ForwardMoveForce => this._forwardMoveForce;
    public float BackMoveForce => this._backMoveForce;
    public float BrakeForce => this._brakeForce;
    public float AirBrakeForce => this._airBrakeForce;
    public float GearBrakeForce => this._gearBrakeForce;
    public float WheelSize => this._wheelSize;

    public float DefaultSuspensionFrequency => this._defaultSuspensionFrequency;
    public float DefaultSuspensionHeight => this._defaultSuspensionHeight;

    public GearType GearType => this._gearType;
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