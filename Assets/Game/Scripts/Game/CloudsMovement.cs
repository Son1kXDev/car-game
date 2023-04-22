using UnityEngine;
using NTC.Global.Cache;


namespace Assets.Game.Scripts.Game
{
    public class CloudsMovement : MonoCache
    {
        [SerializeField, Range(0.1f, 20f)] private float _speed = 1f;

        protected override void LateRun()
        {
            transform.position += _speed * Time.deltaTime * Vector3.left;

            if (transform.position.x < -24f)
                transform.position = new(408f, transform.position.y, transform.position.z);

        }
    }
}