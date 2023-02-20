using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts
{
    public class CarScript : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private Animator _backMoveLights;
        [SerializeField] private Animator _brakeLights;
        [SerializeField] private GameObject _backCasualLights;
        [SerializeField] private GameObject _dippedLights;
        [SerializeField] private GameObject _highBeamLights;
        [SerializeField] private Color _carColor = Color.white;

        [Header("Settings")]
        [SerializeField] private GearType _gearType = GearType.Full;
        [SerializeField] private LayerMask _ground;
        [SerializeField] private Transform _wheel;

        [SerializeField] private float _acceleration = 500f;
        [SerializeField] private float _maxSpeed = 800f;
        [SerializeField] private float _maxBackSpeed = 600f;
        [SerializeField] private float _brakeForce = 1000f;
        [SerializeField] private float _airBrakeForce = 500f;
        [SerializeField] private float _gearBrakeForce = 80f;
        [SerializeField] private float _wheelSize;

        [SerializeField] private List<int> _gearsMaxSpeed = new() { 400, 800, 1200, 1500, 2000, 2200 };
        [SerializeField] private List<float> _maximumMotorForces = new() { 2.5f, 2.25f, 2f, 1.85f, 1.5f, 1.25f };

        [Header("Upgrades")]
        [SerializeField] private float _accelirationMultiplier = 1f;
        [SerializeField] private float _maxSpeedMultiplier = 1f;
        [SerializeField] private float _breakForceMultiplier = 1f;
        [SerializeField] private float _gearSwitchMultiplier = 1f;
        [SerializeField] private float _suspensionFrequencyMultiplier = 1f;
        [SerializeField] private float _suspensionHeightMultiplier = 1f;

        private readonly float _deceleration = -400f;
        private readonly float _gravity = 9.8f;
        private float _carAngle = 0;
        private int _currentGear = 1;
        private bool _grounded;
        private bool _changingGear;
        private WheelJoint2D[] _wheelJoints;
        private JointMotor2D _frontWheel, _backWheel;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _wheelJoints = GetComponents<WheelJoint2D>();
            switch (_gearType)
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

            _highBeamLights.SetActive(true);
            _backCasualLights.SetActive(true);
            transform.Find("Base").GetComponent<SpriteRenderer>().color = _carColor;

            foreach (WheelJoint2D wheel in _wheelJoints)
            {
                JointSuspension2D susp = wheel.suspension;
                susp.frequency *= _suspensionFrequencyMultiplier;
                Vector2 anch = wheel.anchor;
                anch.y *= (0.9f + _suspensionHeightMultiplier / 10);
                wheel.anchor = anch;
                wheel.suspension = susp;
            }
            _maxBackSpeed *= _maxSpeedMultiplier;

            StartCoroutine(ChangeGear());
        }

        private void Update()
        {
            if ((Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") == 0) && !Input.GetKey(KeyCode.Space))
            {
                _backCasualLights.SetActive(true);
                _backMoveLights.SetBool("active", false);
                _brakeLights.SetBool("active", false);
            }

            if (Input.GetAxis("Horizontal") < 0 && !Input.GetKey(KeyCode.Space))
            {
                _backCasualLights.SetActive(false);
                _backMoveLights.SetBool("active", true);
                _brakeLights.SetBool("active", false);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                _brakeLights.SetBool("active", true);
                _backCasualLights.SetActive(false);
                _backMoveLights.SetBool("active", false);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _brakeLights.SetBool("active", false);
                _backCasualLights.SetActive(true);
                _backMoveLights.SetBool("active", false);
            }

            Speedometer();
            Gearbox();
            Tachometer();
        }

        private void Speedometer()
        {
            float speedOnKmh;
            float speedOnMph;

            speedOnKmh = _rigidbody.velocity.magnitude * 7.2f;
            speedOnMph = speedOnKmh * 0.62f;

            UIManager.Instance.DisplaySpeedometer($"{Mathf.Round(speedOnKmh)} km/h");
        }

        private void Tachometer()
        {
            float idleValue = Random.Range(500, 600);
            if (Input.GetAxis("Horizontal") != 0 || Mathf.Round(Mathf.Abs(_backWheel.motorSpeed)) > 30)
                UIManager.Instance.DisplayTachometer($"{Mathf.Abs(Mathf.Round(_backWheel.motorSpeed)) + idleValue} RPM");
            else UIManager.Instance.DisplayTachometer($"{idleValue} RPM");
        }

        private void Gearbox()
        {
            if (Input.GetAxis("Horizontal") > 0) UIManager.Instance.DisplayGearbox($"{_currentGear + 1}");
            else if (Input.GetAxis("Horizontal") == 0) UIManager.Instance.DisplayGearbox("N");
            else UIManager.Instance.DisplayGearbox("R");
        }

        private IEnumerator ChangeGear()
        {
            _currentGear = 0;
            _maxSpeed = _gearsMaxSpeed[_currentGear] * _maxSpeedMultiplier;
            _changingGear = false;
            while (true)
            {
                if (_grounded && Mathf.Abs(_backWheel.motorSpeed) == _gearsMaxSpeed[_currentGear] * _maxSpeedMultiplier
                    && Input.GetAxis("Horizontal") > 0 && _currentGear != _gearsMaxSpeed.Count - 1)
                {
                    _currentGear++;
                    if (_currentGear >= _gearsMaxSpeed.Count)
                        _currentGear = _gearsMaxSpeed.Count - 1;
                    _maxSpeed = _gearsMaxSpeed[_currentGear] * _maxSpeedMultiplier;
                    _changingGear = true;
                    _backWheel.maxMotorTorque = _maximumMotorForces[_currentGear];
                    yield return new WaitForSeconds(1 / _gearSwitchMultiplier);
                    _changingGear = false;
                }
                if ((Input.GetAxis("Horizontal") <= 0 || !_grounded) && _currentGear > 0 &&
                    Mathf.Abs(_backWheel.motorSpeed) < _gearsMaxSpeed[_currentGear - 1] * _maxSpeedMultiplier && !_changingGear)
                {
                    _currentGear--;
                    _maxSpeed = _gearsMaxSpeed[_currentGear] * _maxSpeedMultiplier;
                    _backWheel.maxMotorTorque = _maximumMotorForces[_currentGear];
                }
                yield return null;
            }
        }

        private void SetMotorActive()
        {
            if (Input.GetAxis("Horizontal") == 0 && SpeedIsZero()
                && (_wheelJoints[0].useMotor == true || _wheelJoints[1].useMotor == true) && !Input.GetKey(KeyCode.Space))
            {
                foreach (WheelJoint2D wheel in _wheelJoints)
                    wheel.useMotor = false;
            }

            if (Input.GetAxis("Horizontal") != 0 || Input.GetKey(KeyCode.Space) && _wheelJoints[0].useMotor == false && _wheelJoints[1].useMotor == false)
            {
                switch (_gearType)
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

        private void FixedUpdate()
        {
            _grounded = Physics2D.OverlapCircle(_wheel.transform.position, _wheelSize, _ground);

            _carAngle = transform.localEulerAngles.z;
            if (_carAngle > 180) _carAngle -= 360;

            if (Input.GetKey(KeyCode.Space))
            {
                if (_backWheel.motorSpeed < 0)
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed + _brakeForce * _breakForceMultiplier * Time.fixedDeltaTime, _backWheel.motorSpeed, 0);
                if (_backWheel.motorSpeed > 0)
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed - _brakeForce * _breakForceMultiplier * Time.fixedDeltaTime, 0, _backWheel.motorSpeed);
            }

            if (_grounded == false && !Input.GetKey(KeyCode.Space))
            {
                if (_backWheel.motorSpeed < 0)
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed + _airBrakeForce * Time.fixedDeltaTime, _backWheel.motorSpeed, 0);
                if (_backWheel.motorSpeed > 0)
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed - _airBrakeForce * Time.fixedDeltaTime, 0, _backWheel.motorSpeed);
            }

            if (!_changingGear)
            {
                if (Input.GetAxis("Horizontal") != 0 && !Input.GetKey(KeyCode.Space))
                    _backWheel.motorSpeed = Mathf.Clamp(_backWheel.motorSpeed - (_acceleration * _accelirationMultiplier - _gravity * Mathf.PI * (_carAngle / 180) * 80)
                           * Input.GetAxis("Horizontal") * Time.fixedDeltaTime, -_maxSpeed, _maxBackSpeed);
            }

            if (_grounded)
            {
                if (Input.GetAxis("Horizontal") == 0 && _backWheel.motorSpeed < 0 && !Input.GetKey(KeyCode.Space) || _changingGear)
                {
                    _backWheel.motorSpeed = Mathf.Clamp
                           (_backWheel.motorSpeed - (_deceleration - _gravity * Mathf.PI * (_carAngle / 180) * _gearBrakeForce) * Time.fixedDeltaTime, -_maxSpeed, _maxBackSpeed);
                }

                if (Input.GetAxis("Horizontal") == 0 && _backWheel.motorSpeed > 0 && !Input.GetKey(KeyCode.Space) || _changingGear)
                {
                    _backWheel.motorSpeed = Mathf.Clamp
                           (_backWheel.motorSpeed - (-_deceleration - _gravity * Mathf.PI * (_carAngle / 180) * _gearBrakeForce) * Time.fixedDeltaTime, -_maxSpeed, _maxBackSpeed);
                }
            }

            _frontWheel = _backWheel;

            switch (_gearType)
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_wheel.transform.position, _wheelSize);
        }
    }

    public enum GearType
    { Back, Front, Full }
}