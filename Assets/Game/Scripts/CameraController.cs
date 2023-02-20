using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _zoomStep;
    [SerializeField] private float _zoomWaitTime = 2f;
    [SerializeField] private float _smooth = 15f;
    [SerializeField] private Vector2 _camSize;

    private Camera _camera;
    private Vector3 _dragOrigin;
    private Vector3 _offset;
    private float _baseZoom;

    private bool _isScrolling;
    private bool _isMoving;
    private bool _stopMoving;
    private bool _stopScrolling;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _offset = transform.position - _target.position;
        _baseZoom = _camera.orthographicSize;
        StartCoroutine(CheckZoomState());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
            _isMoving = true;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = _dragOrigin - _camera.ScreenToWorldPoint(Input.mousePosition);
            _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _camera.transform.position + difference, Time.deltaTime * _smooth);
        }

        if (Input.GetMouseButtonUp(0)) _stopMoving = true;

        if (_stopMoving)
        {
            _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _target.position + _offset, Time.deltaTime * _smooth);
            if (_camera.transform.position == _target.position + _offset)
            {
                _stopMoving = false;
                _isMoving = false;
            }
        }

        if (_stopScrolling)
        {
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _baseZoom, Time.deltaTime * _smooth);
            if (_camera.orthographicSize == _baseZoom)
            {
                _stopScrolling = false;
                _isScrolling = false;
            }
        }

        if (Input.mouseScrollDelta.y < 0) ZoomIn();
        if (Input.mouseScrollDelta.y > 0) ZoomOut();

        if (!_isMoving)
            transform.position = _target.position + _offset;
    }

    private IEnumerator CheckZoomState()
    {
        while (true)
        {
            float t = _zoomWaitTime;
            bool timer = false;
            if (Input.mouseScrollDelta.y != 0)
            {
                _isScrolling = true;
                timer = true;
            }
            while (timer)
            {
                if (t >= 0)
                    t -= Time.deltaTime;

                if (Input.mouseScrollDelta.y == 0 && t <= 0)
                {
                    timer = false;
                    _stopScrolling = true;
                }
                yield return null;
            }
            yield return null;
        }
    }

    private void ZoomIn()
    {
        float newSize = _camera.orthographicSize + _zoomStep;
        _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, Mathf.Clamp(newSize, _camSize.x, _camSize.y), Time.deltaTime * _smooth);
    }

    private void ZoomOut()
    {
        float newSize = _camera.orthographicSize - _zoomStep;
        _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, Mathf.Clamp(newSize, _camSize.x, _camSize.y), Time.deltaTime * _smooth);
    }
}