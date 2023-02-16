using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Vector3 _offset;

        private void Awake()
        {
            _offset = transform.position - _target.position;
        }

        private void Update()
        {
            transform.position = _target.position + _offset;
        }
    }
}