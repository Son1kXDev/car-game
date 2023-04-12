using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{

    public static MenuCameraController Instance;

    [SerializeField] Camera _camera;
    [SerializeField] Vector3 _defaultPosition;

    [SerializeField] List<Vector3> _cameraDataList;

    [SerializeField] private float _speed = 10f;

    private float _distance;

    private Vector3 _newPosition;
    bool _changePosition = false;
    bool _changeType = false;


    void Awake()
    {
        if (Instance) Destroy(this.gameObject);
        else Instance = this;
    }

    public void SetCamera(int value)
    {
        SetCameraPosition(_cameraDataList[value]);
    }

    private void SetCameraPosition(Vector3 position)
    {
        _distance = Vector3.Distance(_camera.transform.position, position) * _speed;
        _newPosition = position;
        _changePosition = true;
    }


    void Update()
    {
        if (_changePosition)
        {
            _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _newPosition, Time.deltaTime * _distance);
            if (_camera.transform.position == _newPosition)
            {
                if (_changeType)
                {
                    SetCameraToOrthographic(true);
                    GetComponent<Assets.Game.Scripts.Game.CameraController>().enabled = true;
                    _changeType = false;
                }
                _changePosition = false;
            }
        }
    }


    public void ResetCamera()
    {
        _newPosition = _defaultPosition;
        _changePosition = _changeType = true;
    }

    public void SetCameraToOrthographic(bool value) => _camera.orthographic = value;


}