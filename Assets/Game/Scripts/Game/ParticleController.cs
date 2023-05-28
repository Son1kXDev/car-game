using UnityEngine;
using Assets.Game.Scripts.Data;

public class ParticleController : MonoBehaviour, ISettingsDataPersistence
{
    private void Awake() => UpdateParticles();

    private void Start() => GlobalEventManager.Instance.OnParticleToggleChanged += UpdateParticles;

    private void OnDestroy() => GlobalEventManager.Instance.OnParticleToggleChanged -= UpdateParticles;

    private bool _particles;

    public void UpdateParticles()
    {
        var particles = FindObjectsOfType<ParticleSystem>();
        foreach (var particle in particles)
            particle.enableEmission = _particles;
    }

    public void LoadData(SettingsData data)
    { _particles = data.Particles; }

    public void SaveData(SettingsData data) { }
}
