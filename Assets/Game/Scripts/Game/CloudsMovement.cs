using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class CloudsMovement : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 20f)] private float _speed = 1f;

        private void Update()
        {
            transform.position += _speed * Time.deltaTime * Vector3.left;

            if (transform.position.x < -24f)
                transform.position = new(408f, transform.position.y, transform.position.z);

        }
    }
}