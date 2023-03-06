using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class CameraScale : MonoBehaviour
    {
        public float zoomFactor = 0f;

        [SerializeField] private float _minZoom = 3f, _maxZoom = 6f;
        [SerializeField] private Camera _camera;
        [SerializeField] private RectTransform _background;

        private Touch touchA;
        private Touch touchB;
        private Vector2 touchADirection;
        private Vector2 touchBDirection;
        private float targetZoom;
        private float dstBtwTouchesPosition;
        private float dstBtwTouchesDirections;

        private void Awake()
        {
            targetZoom = _camera.orthographicSize;
        }

        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float scrollData;
                scrollData = -Input.GetAxis("Mouse ScrollWheel");

                targetZoom -= -scrollData * zoomFactor;
                targetZoom = Mathf.Clamp(targetZoom, _minZoom, _maxZoom);

                _camera.orthographicSize = targetZoom;
                ResizeBackground(targetZoom);
            }

            if (Input.touchCount == 2)
            {
                touchA = Input.GetTouch(0);
                touchB = Input.GetTouch(1);
                touchADirection = touchA.position - touchA.deltaPosition;
                touchBDirection = touchB.position - touchB.deltaPosition;

                dstBtwTouchesPosition = Vector2.Distance(touchA.position, touchB.position);
                dstBtwTouchesDirections = Vector2.Distance(touchADirection, touchBDirection);

                targetZoom = dstBtwTouchesPosition - dstBtwTouchesDirections;

                var currentZoom = _camera.orthographicSize - targetZoom * zoomFactor;
                currentZoom = Mathf.Clamp(currentZoom, _minZoom, _maxZoom);

                _camera.orthographicSize = currentZoom;
                ResizeBackground(currentZoom);
            }
        }

        public void ResizeBackground(float zoom)
        {
            Vector2 cashedPosition = _background.anchoredPosition;
            float k = 6.6f;
            float y = k / zoom;
            cashedPosition.y = y;
            _background.anchoredPosition = cashedPosition;
        }
    }
}