using UnityEngine;
using NTC.Global.Cache;

namespace Assets.Game.Scripts.Game
{
    public class BrakesFix : MonoCache
    {
        [SerializeField] private Transform _transform;

        protected override void Run() => transform.rotation = _transform.rotation;
    }
}