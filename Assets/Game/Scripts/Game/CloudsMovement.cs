using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class CloudsMovement : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 20f)] private float _speed = 1f;
        [SerializeField, Range(0, 5)] private float _spawnRange = 1f;
        [SerializeField] private Camera _skyCamera;

        private Vector2 _offset;
        private bool _hasCopy = false;

        private void Awake()
        {
            _offset = _skyCamera.WorldToScreenPoint(GetComponent<SpriteRenderer>().bounds.max) -
                _skyCamera.WorldToScreenPoint(GetComponent<SpriteRenderer>().bounds.min);
        }

        private void Update()
        {
            transform.position += _speed * Time.deltaTime * Vector3.left;

            Vector2 screenPos = _skyCamera.WorldToScreenPoint(transform.position);

            Vector2 min = screenPos - _offset;

            // if (!_hasCopy && (min.x < 0))
            // {
            //     _hasCopy = true;
            //     Vector2 spawnPosition = _skyCamera.ScreenToWorldPoint(new Vector3(Screen.width * 1.5f, screenPos.y, 0));
            //     spawnPosition.y += Random.Range(-_spawnRange, _spawnRange);
            //     GameObject cloud = Instantiate(gameObject, spawnPosition, Quaternion.identity, transform.parent);
            //     cloud.name = "Cloud";
            // }

            if (transform.position.x < -24f)
                transform.position = new(408f, transform.position.y, transform.position.z);

        }
    }
}