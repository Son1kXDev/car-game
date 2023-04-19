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

        private Camera _camera;
        private Vector3 _offset;

        private bool _inputLocked;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _offset = transform.position - _target.position;
        }

        private void Update()
        {
            if (Input.touchCount == 2 && !_inputLocked)
            {
                Touch touch = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                Vector2 touchPreviousPosition = touch.position - touch.deltaPosition;
                Vector2 touch2PreviousPosition = touch2.position - touch2.deltaPosition;

                float previousMagnitude = (touchPreviousPosition - touch2PreviousPosition).magnitude;
                float currentMagnitude = (touch.position - touch2.position).magnitude;

                float difference = currentMagnitude - previousMagnitude;

                Zoom(difference * 0.001f * _zoomStep);
            }

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