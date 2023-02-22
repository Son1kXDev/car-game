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
    private bool _inputLocked;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _offset = transform.position - _target.position;
        _baseZoom = _camera.orthographicSize;
        StartCoroutine(CheckZoomState());
    }

    private void Update()
    {
        if (Input.touchCount > 0 && !_inputLocked)
        {
            Touch touch = Input.GetTouch(0);
            if (Input.touchCount == 1 && touch.phase == TouchPhase.Began)
            {
                _dragOrigin = _camera.ScreenToWorldPoint(touch.position);
                _isMoving = true;
            }
            if (Input.touchCount == 1 && touch.phase == TouchPhase.Moved)
            {
                Vector3 difference = _dragOrigin - _camera.ScreenToWorldPoint(touch.position);
                _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _camera.transform.position + difference, Time.deltaTime * _smooth);
            }

            if (Input.touchCount == 1 && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                _stopMoving = true;

            if (Input.touchCount == 2)
            {
                _isScrolling = true;
                Touch touch2 = Input.GetTouch(1);

                Vector2 touchPreviousPosition = touch.position - touch.deltaPosition;
                Vector2 touch2PreviousPosition = touch2.position - touch2.deltaPosition;

                float previousMagnitude = (touchPreviousPosition - touch2PreviousPosition).magnitude;
                float currentMagnitude = (touch.position - touch2.position).magnitude;

                float difference = currentMagnitude - previousMagnitude;

                Zoom(difference * 0.001f * _zoomStep);
            }
        }

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

        if (!_isMoving)
            transform.position = _target.position + _offset;
    }

    public void LockInput(bool value) => _inputLocked = value;

    private IEnumerator CheckZoomState()
    {
        while (true)
        {
            float t = _zoomWaitTime;
            bool timer = false;
            if (_isScrolling)
                timer = true;

            while (timer)
            {
                if (t >= 0)
                    t -= Time.deltaTime;

                if (t < 0)
                {
                    if (Input.touchCount == 0)
                    {
                        timer = false;
                        _stopScrolling = true;
                    }
                    else t = _zoomWaitTime;
                }
                yield return null;
            }
            yield return null;
        }
    }

    private void Zoom(float increment)
    {
        float newSize = _camera.orthographicSize - increment;
        _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, Mathf.Clamp(newSize, _camSize.x, _camSize.y), Time.deltaTime * _smooth);
    }
}