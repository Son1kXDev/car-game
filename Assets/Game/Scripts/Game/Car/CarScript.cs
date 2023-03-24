﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    [RequireComponent(typeof(Rigidbody2D), typeof(WheelJoint2D), typeof(WheelJoint2D)), RequireComponent(typeof(CarVisual))]
    public class CarScript : MonoBehaviour, Data.IDataPersistence
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _ground;
        [SerializeField] private Transform _wheel;
        [SerializeField] private Car _carAsset;

        private readonly float _deceleration = -400f;
        private readonly float _gravity = 9.8f;
        private float _carAngle = 0;
        private float _moveDirection = 1f;
        private float _maxSpeed;
        private float _maxBackSpeed;
        private int _currentGear = 1;
        private bool _grounded;
        private bool _changingGear;
        private bool _switchGear;
        private bool _brake;
        private bool _move;
        private WheelJoint2D[] _wheelJoints;
        private JointMotor2D _frontWheel, _backWheel;
        private Rigidbody2D _rigidbody;
        private CarVisual _carVisual;
        private Upgrades _upgrades;

        private IEnumerator Start()
        {
            while (!Data.DataPersistenceManager.Loaded) yield return null;
            _carVisual = GetComponent<CarVisual>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _wheelJoints = GetComponents<WheelJoint2D>();
            _maxSpeed = _carAsset.MaxSpeed;
            _maxBackSpeed = _carAsset.MaxBackSpeed;
            _maxBackSpeed *= _upgrades.MaxSpeedMultiplier;
            foreach (WheelJoint2D wheel in _wheelJoints)
            {
                JointMotor2D motor = wheel.motor;
                motor.maxMotorTorque = _upgrades.EngineMultiplier;
                JointSuspension2D susp = wheel.suspension;
                susp.frequency *= _upgrades.SuspensionFrequencyMultiplier;
                Vector2 anch = wheel.anchor;
                anch.y *= (0.9f + _upgrades.SuspensionHeightMultiplier / 10);
                wheel.anchor = anch;
                wheel.suspension = susp;
                wheel.motor = motor;
            }
            switch (_carAsset.GearType)
            {
                case GearType.Back:
                    _backWheel = _wheelJoints[1].motor;
                    break;

                case GearType.Front:
                    _backWheel = _wheelJoints[0].motor;
                    break;

                case GearType.Full:
                    _frontWheel = _wheelJoints[0].motor;
                    _backWheel = _wheelJoints[1].motor;
                    break;
            }
            StartCoroutine(ChangeGear());
        }

        private void PCControll()
        {
            if (Input.GetKey(KeyCode.D))
            {
                ButtonMove(true);
                _moveDirection = 1f;
            }
            if (Input.GetKeyUp(KeyCode.D)) ButtonMove(false);
            if (Input.GetKey(KeyCode.A))
            {
                ButtonMove(true);
                _moveDirection = -1f;
            }
            if (Input.GetKeyUp(KeyCode.A)) ButtonMove(true);
            if (Input.GetKey(KeyCode.Space)) ButtonBrake(true);
            if (Input.GetKeyUp(KeyCode.Space)) ButtonBrake(false);
        }

        public float MoveAxis()
        {
            if (_move) return _moveDirection;
            return 0;
        }

        private void Speedometer()
        {
            float speedOnKmh;
            float speedOnMph;

            speedOnKmh = _rigidbody.velocity.magnitude * 7.2f;
            speedOnMph = speedOnKmh * 0.62f;

            UI.UIManager.Instance.DisplaySpeedometer($"{Mathf.Round(speedOnKmh)} km/h");
        }

        private void Tachometer()
        {
            float idleValue = Random.Range(500, 600);
            if (MoveAxis() != 0 || Mathf.Round(Mathf.Abs(_backWheel.motorSpeed)) > 30)
                UI.UIManager.Instance.DisplayTachometer($"{Mathf.Abs(Mathf.Round(_backWheel.motorSpeed)) + idleValue} RPM");
            else UI.UIManager.Instance.DisplayTachometer($"{idleValue} RPM");
        }

        private void Gearbox()
        {
            if (_switchGear) UI.UIManager.Instance.DisplayGearbox("N");
            else if (_moveDirection > 0) UI.UIManager.Instance.DisplayGearbox($"{_currentGear + 1}");
            else if (_moveDirection < 0) UI.UIManager.Instance.DisplayGearbox("R");
        }

        private IEnumerator ChangeGear()
        {
            _currentGear = 0;
            _maxSpeed = _carAsset.GearsMaxSpeed[_currentGear] * _upgrades.MaxSpeedMultiplier;
            _changingGear = false;
            while (true)
            {
                if (_grounded && Mathf.Abs(_backWheel.motorSpeed) == _carAsset.GearsMaxSpeed[_currentGear] * _upgrades.MaxSpeedMultiplier
                    && MoveAxis() > 0 && _currentGear != _carAsset.GearsMaxSpeed.Count - 1)
                {
                    _currentGear++;
                    if (_currentGear >= _carAsset.GearsMaxSpeed.Count)
                        _currentGear = _carAsset.GearsMaxSpeed.Count - 1;
                    _maxSpeed = _carAsset.GearsMaxSpeed[_currentGear] * _upgrades.MaxSpeedMultiplier;
                    _changingGear = true;
                    _backWheel.maxMotorTorque = _carAsset.MaximumMotorForces[_currentGear] * _upgrades.EngineMultiplier;
                    yield return new WaitForSeconds(1 / _upgrades.GearSwitchMultiplier);
                    _changingGear = false;
                }
                if (_currentGear > 0 && (MoveAxis() <= 0 || !_grounded) && Mathf.Abs(_backWheel.motorSpeed) < _carAsset.GearsMaxSpeed[_currentGear - 1] * _upgrades.MaxSpeedMultiplier
                    && !_changingGear)
                {
                    _currentGear--;
                    _maxSpeed = _carAsset.GearsMaxSpeed[_currentGear] * _upgrades.MaxSpeedMultiplier;
                    _backWheel.maxMotorTorque = _carAsset.MaximumMotorForces[_currentGear] * _upgrades.EngineMultiplier;
                }
                yield return null;
            }
        }

        private void SetMotorActive()
        {
            if (MoveAxis() == 0 && SpeedIsZero()
                && (_wheelJoints[0].useMotor == true || _wheelJoints[1].useMotor == true) && !_brake)
            {
                foreach (WheelJoint2D wheel in _wheelJoints)
                    wheel.useMotor = false;
            }

            if (MoveAxis() != 0 || _brake && _wheelJoints[0].useMotor == false && _wheelJoints[1].useMotor == false)
            {
                switch (_carAsset.GearType)
                {
                    case GearType.Back:
                        _wheelJoints[1].useMotor = true;
                        break;

                    case GearType.Front:
                        _wheelJoints[0].useMotor = true;
                        break;

                    case GearType.Full:
                        _wheelJoints[0].useMotor = true;
                        _wheelJoints[1].useMotor = true;
                        break;
                }
            }
        }

        private bool SpeedIsZero()
        {
            if (Mathf.Round(Mathf.Abs(_backWheel.motorSpeed)) == 0) return true;
            if (Mathf.Round(Mathf.Abs(_backWheel.motorSpeed)) < 30 && Mathf.Round(Mathf.Abs(_backWheel.motorSpeed)) > 0) return true;
            if (Mathf.Round(Mathf.Abs(_backWheel.motorSpeed)) > -30 && Mathf.Round(Mathf.Abs(_backWheel.motorSpeed)) < 0) return true;

            return false;
        }

        private void Update()
        {
            if (!Data.DataPersistenceManager.Loaded) return;

            Speedometer();
            Gearbox();
            Tachometer();
            PCControll();
        }

        private void LateUpdate()
        {
            if (!Data.DataPersistenceManager.Loaded) return;
            _carVisual.SetLight(MoveAxis(), _brake);
        }

        private void FixedUpdate()
        {
            if (!Data.DataPersistenceManager.Loaded) return;

            _grounded = Physics2D.OverlapCircle(_wheel.transform.position, _carAsset.WheelSize, _ground);

            _carAngle = transform.localEulerAngles.z;
            if (_carAngle > 180) _carAngle -= 360;

            if (_brake)
            {
                if (_backWheel.motorSpeed < 0)
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed + _carAsset.BrakeForce * _upgrades.BreakForceMultiplier * Time.fixedDeltaTime, _backWheel.motorSpeed, 0);
                if (_backWheel.motorSpeed > 0)
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed - _carAsset.BrakeForce * _upgrades.BreakForceMultiplier * Time.fixedDeltaTime, 0, _backWheel.motorSpeed);
            }

            if (_grounded == false && !_brake)
            {
                if (_backWheel.motorSpeed < 0)
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed + _carAsset.AirBrakeForce * Time.fixedDeltaTime, _backWheel.motorSpeed, 0);
                if (_backWheel.motorSpeed > 0)
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed - _carAsset.AirBrakeForce * Time.fixedDeltaTime, 0, _backWheel.motorSpeed);
            }

            if (!_changingGear)
            {
                if (MoveAxis() != 0 && !_brake)
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed - (_carAsset.Acceleration * _upgrades.AccelirationMultiplier - _gravity * Mathf.PI * (_carAngle / 180) * 80)
                           * MoveAxis() * Time.fixedDeltaTime, -_maxSpeed, _maxBackSpeed);
            }

            if (_grounded)
            {
                if (MoveAxis() == 0 && _backWheel.motorSpeed < 0 && !_brake || _changingGear)
                {
                    _backWheel.motorSpeed = Mathf.Clamp
                           (_backWheel.motorSpeed - (_deceleration - _gravity * Mathf.PI * (_carAngle / 180) * _carAsset.GearBrakeForce) * Time.fixedDeltaTime, -_maxSpeed, _maxBackSpeed);
                }

                if (MoveAxis() == 0 && _backWheel.motorSpeed > 0 && !_brake || _changingGear)
                {
                    _backWheel.motorSpeed = Mathf.Clamp
                           (_backWheel.motorSpeed - (-_deceleration - _gravity * Mathf.PI * (_carAngle / 180) * _carAsset.GearBrakeForce) * Time.fixedDeltaTime, -_maxSpeed, _maxBackSpeed);
                }
            }

            _frontWheel = _backWheel;

            switch (_carAsset.GearType)
            {
                case GearType.Back:
                    _wheelJoints[0].motor = _backWheel;
                    break;

                case GearType.Front:
                    _wheelJoints[1].motor = _frontWheel;
                    break;

                case GearType.Full:
                    _wheelJoints[0].motor = _backWheel;
                    _wheelJoints[1].motor = _frontWheel;
                    break;
            }
            SetMotorActive();
        }

        public void ButtonBrake(bool value) => _brake = value;

        public void SwitchDirection() => _moveDirection *= -1f;

        public void ButtonSwitchGear(bool value) => _switchGear = value;

        public void ButtonMove(bool value) => _move = value;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (_carAsset != null) Gizmos.DrawWireSphere(_wheel.transform.position, _carAsset.WheelSize);
        }

        public void LoadData(GameData data)
        {
            _upgrades = data.CurrentCarUpgrades;
        }

        public void SaveData(GameData data)
        {
        }
    }
}

public enum GearType
{ Back, Front, Full }

public enum SpeedType
{ Kmh, Mph }