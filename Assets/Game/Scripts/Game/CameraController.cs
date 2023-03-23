using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _zoomStep;
        [SerializeField] private float _smooth = 15f;
        [SerializeField] private Vector2 _camSize;
        [SerializeField] private bool _useDrag = true;

        private Camera _camera;
        private Vector3 _dragOrigin;
        private Vector3 _offset;

        private bool _isMoving;
        private bool _stopMoving;
        private bool _inputLocked;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _offset = transform.position - _target.position;
        }

        private void Update()
        {
            if (Input.touchCount > 0 && !_inputLocked)
            {
                Touch touch = Input.GetTouch(0);
                if (_useDrag && Input.touchCount == 1 && touch.phase == TouchPhase.Began)
                {
                    _dragOrigin = _camera.ScreenToWorldPoint(touch.position);
                    _isMoving = true;
                }
                if (_useDrag && Input.touchCount == 1 && touch.phase == TouchPhase.Moved)
                {
                    Vector3 difference = _dragOrigin - _camera.ScreenToWorldPoint(touch.position);
                    _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _camera.transform.position + difference, Time.deltaTime * _smooth);
                }

                if (_useDrag && Input.touchCount == 1 && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                    _stopMoving = true;

                if (Input.touchCount == 2)
                {
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

            if (!_isMoving)
                transform.position = _target.position + _offset;
        }

        public void LockInput(bool value) => _inputLocked = value;

        private void Zoom(float increment)
        {
            float newSize = _camera.orthographicSize - increment;
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, Mathf.Clamp(newSize, _camSize.x, _camSize.y), Time.deltaTime * _smooth);
        }
    }
}