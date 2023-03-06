using UnityEngine;

namespace Assets.Game.Scripts
{
    public class GearSwitchButton : MonoBehaviour
    {
        private Animator _animator;
        private float _direction = 1;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SwitchDirecton()
        {
            _direction *= -1f;
            _animator.SetInteger("Direction", (int)_direction);
        }
    }
}