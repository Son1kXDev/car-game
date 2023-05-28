using UnityEngine;

namespace Assets.Game.Scripts.UI
{
    public class ParticleButton : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake() => _particleSystem = GetComponentInChildren<ParticleSystem>(true);

        public void ToggleParticle(bool toggle)
        {
            if (toggle)
                _particleSystem.Play();
            else _particleSystem.Stop();
        }

    }
}
