using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private void Awake() => UpdateParticles();

    private void Start() => GlobalEventManager.Instance.OnParticleToggleChanged += UpdateParticles;

    private void OnDestroy() => GlobalEventManager.Instance.OnParticleToggleChanged -= UpdateParticles;

    public void UpdateParticles()
    {
        var particles = FindObjectsOfType<ParticleSystem>();
        foreach (var particle in particles)
            particle.enableEmission = bool.Parse(PlayerPrefs.GetString("particlesToggle", "true"));
    }
}
