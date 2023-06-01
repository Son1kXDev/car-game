using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTC.Global.Cache;

namespace Assets.Game.Scripts.Game
{
    public class CameraController : MonoCache
    {
        [SerializeField, StatusIcon(offset: 20)] private Transform _target;
        [SerializeField, Min(0.1f)] private float _zoomStep;
        [SerializeField, Min(0.1f)] private float _smooth = 15f;
        [SerializeField] private Vector2 _camSize;

        private Camera _camera;
        private Vector3 _offset;

        private bool _inputLocked;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _offset = transform.position - _target.position;

            GlobalEventManager.Instance.OnBrakeButtonPressed += LockInput;
            GlobalEventManager.Instance.OnGasButtonPressed += LockInput;
            GlobalEventManager.Instance.OnGearButtonPressed += LockInput;
            GlobalEventManager.Instance.OnLightButtonPressed += LockInput;
            GlobalEventManager.Instance.OnPauseButtonPressed += LockInput;
        }

        private void OnDestroy()
        {
            GlobalEventManager.Instance.OnBrakeButtonPressed -= LockInput;
            GlobalEventManager.Instance.OnGasButtonPressed -= LockInput;
            GlobalEventManager.Instance.OnGearButtonPressed -= LockInput;
            GlobalEventManager.Instance.OnLightButtonPressed -= LockInput;
            GlobalEventManager.Instance.OnPauseButtonPressed -= LockInput;
        }

        protected override void Run()
        {
            transform.position = _target.position + _offset;

            if (_inputLocked) return;

            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount == 2)
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
            }
            else Zoom(Input.GetAxis("Mouse ScrollWheel") * _zoomStep);

        }

        public void LockInput(bool value) => _inputLocked = value;

        private void Zoom(float increment)
        {
            float newSize = _camera.orthographicSize - increment;
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, Mathf.Clamp(newSize, _camSize.x, _camSize.y), Time.deltaTime * _smooth);
        }
    }
}