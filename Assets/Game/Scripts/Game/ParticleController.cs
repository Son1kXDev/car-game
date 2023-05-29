using UnityEngine;
using Assets.Game.Scripts.Data;

public class ParticleController : MonoBehaviour, ISettingsDataPersistence
{
    public static bool Particles { get; private set; }

    [System.Obsolete]
    private void Start()
    {
        GlobalEventManager.Instance.OnParticleToggleChanged += UpdateParticles;
        UpdateParticles();
    }

    [System.Obsolete]
    private void OnDestroy() => GlobalEventManager.Instance.OnParticleToggleChanged -= UpdateParticles;

    [System.Obsolete]
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
