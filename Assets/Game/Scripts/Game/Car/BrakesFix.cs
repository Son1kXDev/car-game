using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class BrakesFix : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private void Update() => transform.rotation = _transform.rotation;
    }
}