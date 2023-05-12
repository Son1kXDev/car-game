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
    [SerializeField] private float _forwardMoveForce = 800f;
    [SerializeField] private float _backMoveForce = 300f;
    [SerializeField] private float _brakeForce = 500f;
    [SerializeField] private float _wheelSize = 0.03f;
    [SerializeField] private float _defaultSuspensionFrequency = 4f;
    [SerializeField] private float _defaultSuspensionHeight = -0.219f;

    [Header("Gear")]
    [SerializeField] private GearType _gearType = GearType.Full;

    public string ID => this._id;
    public float ForwardMoveForce => this._forwardMoveForce;
    public float BackMoveForce => this._backMoveForce;
    public float BrakeForce => this._brakeForce;
    public float WheelSize => this._wheelSize;

    public float DefaultSuspensionFrequency => this._defaultSuspensionFrequency;
    public float DefaultSuspensionHeight => this._defaultSuspensionHeight;

    public GearType GearType => this._gearType;
}

[System.Serializable]
public class Upgrades
{
    public float EngineMultiplier;
    public float MoveForceMultiplier;
    public float BreakForceMultiplier;
    public float SuspensionFrequencyMultiplier;
    public float SuspensionHeightMultiplier;

    public Upgrades()
    {
        EngineMultiplier = 1;
        MoveForceMultiplier = 1;
        BreakForceMultiplier = 1;
        SuspensionFrequencyMultiplier = 1;
        SuspensionHeightMultiplier = 1;
    }

    public Upgrades(float engine, float maxSpeed, float breakForce, float suspensionFrequency, float suspensionHeight)
    {
        EngineMultiplier = engine;
        MoveForceMultiplier = maxSpeed;
        BreakForceMultiplier = breakForce;
        SuspensionFrequencyMultiplier = suspensionFrequency;
        SuspensionHeightMultiplier = suspensionHeight;
    }
}