using UnityEngine;
using Assets.Game.Scripts.Data;

public class ParticleController : MonoBehaviour, ISettingsDataPersistence
{
    public static bool Particles { get; private set; }

    private void Start()
    {
        GlobalEventManager.Instance.OnParticleToggleChanged += UpdateParticles;
        UpdateParticles();
    }

    private void OnDestroy() => GlobalEventManager.Instance.OnParticleToggleChanged -= UpdateParticles;

    public void UpdateParticles()
    {
        DataPersistenceManager.Instance.LoadSettings();
        var particles = FindObjectsOfType<ParticleSystem>(true);
        foreach (var particle in particles)
            particle.enableEmission = Particles;
    }

    public void LoadData(SettingsData data)
    { Particles = data.Particles; }

    public void SaveData(SettingsData data) { }
}
