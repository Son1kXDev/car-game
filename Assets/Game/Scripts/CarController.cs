using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Animator _backMoveLights;
    [SerializeField] private Animator _brakeLights;
    [SerializeField] private GameObject _backCasualLights;
    [SerializeField] private GameObject _dippedLights;
    [SerializeField] private GameObject _highBeamLights;

    [Header("Settings")]
    [SerializeField] private Type type = Type.Back;
    [SerializeField] private WheelJoint2D _wheelBack, _wheelFront;
    [SerializeField] private float _forwardSpeed, _backSpeed;

    private JointMotor2D _jointMotorBack, _jointMotorFront;
    private float _currentSpeed;
    private float _currentGear;
    private float _newSpeed;
    private float _speedFactor;
    private bool _changeSpeed;

    private void Start()
    {
        _highBeamLights.SetActive(true);
        _backCasualLights.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _speedFactor = 3f;
            _brakeLights.SetBool("active", true);
            _backCasualLights.SetActive(false);
            switch (type)
            {
                case Type.Back:
                    BackBreak();
                    break;

                case Type.Front:
                    FrontBreak();

                    break;

                case Type.Full:
                    BackBreak();
                    FrontBreak();
                    break;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _speedFactor = 1f;
            _brakeLights.SetBool("active", false);
            _backCasualLights.SetActive(true);
            _backMoveLights.SetBool("active", false);
            switch (type)
            {
                case Type.Back:
                    BackStopBreak();
                    break;

                case Type.Front:
                    FrontStopBreak();
                    break;

                case Type.Full:
                    BackStopBreak();
                    FrontStopBreak();
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && !Input.GetKey(KeyCode.Space))
        {
            _speedFactor = 1f;
            switch (type)
            {
                case Type.Back:
                    BackWheel(true, _forwardSpeed);
                    break;

                case Type.Front:
                    FrontWheel(true, _forwardSpeed);
                    break;

                case Type.Full:
                    BackWheel(true, _forwardSpeed);
                    FrontWheel(true, _forwardSpeed);
                    break;
            }
        }

        if (Input.GetKeyUp(KeyCode.D) && !Input.GetKey(KeyCode.Space))
        {
            _speedFactor = 1f;
            _backCasualLights.SetActive(true);
            _backMoveLights.SetBool("active", false);
            _brakeLights.SetBool("active", false);
            switch (type)
            {
                case Type.Back:
                    BackWheel(false, 0);
                    break;

                case Type.Front:
                    FrontWheel(false, 0);
                    break;

                case Type.Full:
                    BackWheel(false, 0);
                    FrontWheel(false, 0);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.Space))
        {
            _speedFactor = 1f;
            _backMoveLights.SetBool("active", true);
            _backCasualLights.SetActive(false);
            switch (type)
            {
                case Type.Back:
                    BackWheel(true, _backSpeed);
                    break;

                case Type.Front:
                    FrontWheel(true, _backSpeed);
                    break;

                case Type.Full:
                    BackWheel(true, _backSpeed);
                    FrontWheel(true, _backSpeed);
                    break;
            }
        }

        if (Input.GetKeyUp(KeyCode.A) && !Input.GetKey(KeyCode.Space))
        {
            _speedFactor = 1f;
            _backMoveLights.SetBool("active", false);
            _backCasualLights.SetActive(true);
            switch (type)
            {
                case Type.Back:
                    BackWheel(false, 0);
                    break;

                case Type.Front:
                    FrontWheel(false, 0);
                    break;

                case Type.Full:
                    BackWheel(false, 0);
                    FrontWheel(false, 0);
                    break;
            }
        }

        if (_changeSpeed)
            ChangeSpeed();
    }

    public void BackWheel(bool move, float speed)
    {
        _newSpeed = speed;
        _changeSpeed = true;
        _wheelBack.useMotor = move;
    }

    public void FrontWheel(bool move, float speed)
    {
        _newSpeed = speed;
        _changeSpeed = true;
        _wheelFront.useMotor = move;
    }

    public void BackBreak()
    {
        _wheelBack.useMotor = true;
        _newSpeed = 0f;
        _changeSpeed = true;
    }

    public void BackStopBreak() => _wheelBack.useMotor = false;

    public void FrontBreak()
    {
        _wheelFront.useMotor = true;
        _newSpeed = 0f;
        _changeSpeed = true;
    }

    public void FrontStopBreak() => _wheelFront.useMotor = false;

    private void ChangeSpeed()
    {
        if (Mathf.Round(_currentSpeed) != Mathf.Round(_newSpeed))
        {
            float delta = _newSpeed - _currentSpeed;
            delta *= Time.deltaTime * _speedFactor;
            _currentSpeed += delta;
            SetSpeed();
        }
        else _changeSpeed = false;
    }

    private void SetSpeed()
    {
        switch (type)
        {
            case Type.Back:
                _jointMotorBack = _wheelBack.motor;
                _jointMotorBack.motorSpeed = Mathf.Round(_currentSpeed);
                _jointMotorBack.motorSpeed = Mathf.Round(_currentSpeed);
                _wheelBack.motor = _jointMotorBack;
                break;

            case Type.Front:
                _jointMotorFront = _wheelFront.motor;
                _jointMotorFront.motorSpeed = Mathf.Round(_currentSpeed);
                _jointMotorFront.motorSpeed = Mathf.Round(_currentSpeed);
                _wheelFront.motor = _jointMotorFront;
                break;

            case Type.Full:
                _jointMotorBack = _wheelBack.motor;
                _jointMotorBack.motorSpeed = Mathf.Round(_currentSpeed);
                _jointMotorBack.motorSpeed = Mathf.Round(_currentSpeed);
                _wheelBack.motor = _jointMotorBack;
                _jointMotorFront = _wheelFront.motor;
                _jointMotorFront.motorSpeed = Mathf.Round(_currentSpeed);
                _jointMotorFront.motorSpeed = Mathf.Round(_currentSpeed);
                _wheelFront.motor = _jointMotorFront;
                break;
        }
    }

    private void Transmission()
    {
    }
}

public enum Type
{ Back, Front, Full }