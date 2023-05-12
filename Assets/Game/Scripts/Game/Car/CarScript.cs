using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTC.Global.Cache;
using FMOD.Studio;

namespace Assets.Game.Scripts.Game
{
    [RequireComponent(typeof(Rigidbody2D), typeof(WheelJoint2D), typeof(WheelJoint2D)), RequireComponent(typeof(CarVisual))]
    public class CarScript : MonoCache, Data.IDataPersistence
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _ground;
        [SerializeField] private Transform _wheel;
        [SerializeField] private Car _carAsset;
        [SerializeField] private Rigidbody2D _backWheelRigidbody, _frontWheelRigidbody;

        private float _moveDirection = 1f;
        private float _motorTemperature = 80f;
        private bool _grounded;
        private bool _changingGear;
        private bool _brake;
        private bool _move;
        private WheelJoint2D[] _wheelJoints;
        private JointMotor2D _frontWheel, _backWheel;
        private Rigidbody2D _carRigidbody;
        private CarVisual _carVisual;
        private Upgrades _upgrades;
        private EventInstance _engineSoundInstance;

        private IEnumerator Start()
        {
            while (!Data.DataPersistenceManager.Loaded) yield return null;
            _carVisual = GetComponent<CarVisual>();
            _carRigidbody = GetComponent<Rigidbody2D>();
            _wheelJoints = GetComponents<WheelJoint2D>();
            _motorTemperature = 80f;
            foreach (WheelJoint2D wheel in _wheelJoints)
            {
                JointMotor2D motor = wheel.motor;
                motor.maxMotorTorque = _upgrades.EngineMultiplier;
                JointSuspension2D susp = wheel.suspension;
                susp.frequency = _carAsset.DefaultSuspensionFrequency * _upgrades.SuspensionFrequencyMultiplier;
                Vector2 anch = wheel.anchor;
                anch.y = _carAsset.DefaultSuspensionHeight * (0.9f + _upgrades.SuspensionHeightMultiplier / 10);
                wheel.anchor = anch;
                wheel.suspension = susp;
                wheel.motor = motor;
                wheel.useMotor = false;
            }
            _engineSoundInstance = AudioManager.Instance.CreateEventInstance(Audio.Data.Engine);
            _engineSoundInstance.start();
        }

        private void OnDestroy() { _engineSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE); }

        public float MoveAxis()
        {
            if (_move) return _moveDirection;
            return 0;
        }

        #region DISPLAY DATA

        private void Speedometer()
        {
            float speedOnKmh;
            float speedOnMph;

            speedOnKmh = _carRigidbody.velocity.magnitude * 7.2f;
            speedOnMph = speedOnKmh * 0.62f;

            UI.UIManager.Instance.DisplaySpeedometer($"{Mathf.Round(speedOnKmh)} km/h");
        }
        private void Temperature()
        {
            if (MoveAxis() != 0)
                _motorTemperature = Mathf.Clamp(_motorTemperature + Time.deltaTime / 2, 80, 110);
            else _motorTemperature = Mathf.Clamp(_motorTemperature - Time.deltaTime / 4, 80, 110);

            UI.UIManager.Instance.DisplayTemperature($"{Mathf.Round(_motorTemperature)} C\u00B0");
        }
        private void Gearbox() { UI.UIManager.Instance.DisplayGearbox(_moveDirection > 0 ? "D" : "R"); }

        #endregion

        #region UPDATE

        protected override void Run()
        {
            if (!Data.DataPersistenceManager.Loaded) return;

            Speedometer();
            Gearbox();
            Temperature();
            _engineSoundInstance.setParameterByName("RPM", Mathf.Abs(Mathf.Round(_backWheel.motorSpeed)));
        }

        protected override void LateRun()
        {
            if (!Data.DataPersistenceManager.Loaded) return;
            _carVisual.SetLight(MoveAxis(), _brake);
        }

        protected override void FixedRun()
        {
            if (!Data.DataPersistenceManager.Loaded) return;

            _grounded = Physics2D.OverlapCircle(_wheel.transform.position, _carAsset.WheelSize, _ground);
            if (!_grounded) return;

            if (MoveAxis() > 0)
            {
                _backWheelRigidbody.AddTorque(-MoveAxis() * _carAsset.ForwardMoveForce * Time.fixedDeltaTime);
                _frontWheelRigidbody.AddTorque(-MoveAxis() * _carAsset.ForwardMoveForce * Time.fixedDeltaTime);
                _carRigidbody.AddTorque(MoveAxis() * _carAsset.ForwardMoveForce / 2 * Time.fixedDeltaTime);
            }
            else if (MoveAxis() < 0)
            {
                _backWheelRigidbody.AddTorque(-MoveAxis() * _carAsset.BackMoveForce * Time.fixedDeltaTime);
                _frontWheelRigidbody.AddTorque(-MoveAxis() * _carAsset.BackMoveForce * Time.fixedDeltaTime);
                _carRigidbody.AddTorque(MoveAxis() * _carAsset.BackMoveForce / 2 * Time.fixedDeltaTime);
            }
            if (_brake)
            {
                if (Mathf.Abs(_backWheelRigidbody.angularVelocity) > 50)
                {
                    _backWheelRigidbody.angularVelocity =
                    Mathf.Lerp(_backWheelRigidbody.angularVelocity, 0, _carAsset.BrakeForce * Time.fixedDeltaTime);
                    _frontWheelRigidbody.angularVelocity =
                    Mathf.Lerp(_frontWheelRigidbody.angularVelocity, 0, _carAsset.BrakeForce * Time.fixedDeltaTime);
                    _carRigidbody.angularVelocity =
                    Mathf.Lerp(_carRigidbody.angularVelocity, 0, _carAsset.BrakeForce * Time.fixedDeltaTime);
                }
                else if (_wheelJoints[0].useMotor == false)
                {
                    _backWheelRigidbody.angularVelocity = 0;
                    _frontWheelRigidbody.angularVelocity = 0;
                    _carRigidbody.angularVelocity = 0;
                    _backWheel.motorSpeed = 0;
                    _frontWheel.motorSpeed = 0;
                    _wheelJoints[0].useMotor = true;
                    _wheelJoints[1].useMotor = true;
                }
            }
            else if (_wheelJoints[0].useMotor == true)
            {
                _wheelJoints[0].useMotor = false;
                _wheelJoints[1].useMotor = false;
            }
        }

        #endregion

        public void ButtonBrake(bool value) => _brake = value;
        public void ButtonMove(bool value) => _move = value;

        public void SwitchDirection()
        {
            _moveDirection *= -1f;
            AudioManager.Instance.PlayOneShot(Audio.Data.GearSwitch, transform.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (_carAsset != null) Gizmos.DrawWireSphere(_wheel.transform.position, _carAsset.WheelSize);
        }

        #region LOADING
        public void LoadData(GameData data)
        { _upgrades = data.CarUpgrades[data.CurrentCar]; }

        public void SaveData(GameData data) { }
        #endregion
    }
}

public enum GearType
{ Back, Front, Full }

public enum SpeedType
{ Kmh, Mph }