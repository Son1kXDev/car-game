using UnityEngine;
using NTC.Global.Cache;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoCache
    {
        [SerializeField, StatusIcon] private Camera _target;
        private Camera _camera;

        private void Awake() => _camera = GetComponent<Camera>();

        protected override void LateRun()
        {
            transform.position = _target.transform.position;
            _camera.orthographicSize = _target.orthographicSize;
            _camera.orthographic = _target.orthographic;
        }
    }
}