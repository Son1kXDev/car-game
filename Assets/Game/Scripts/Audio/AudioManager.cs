using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
using Assets.Game.Scripts.Data;

public class AudioManager : MonoBehaviour, ISettingsDataPersistence
{
    public static AudioManager Instance { get; private set; }
    [HideInInspector] public float MasterVolume = 1f;
    [HideInInspector] public float MusicVolume = 1f;
    [HideInInspector] public float SFXVolume = 1f;
    [HideInInspector] public float AmbientVolume = 1f;
    [HideInInspector] public float UIVolume = 1f;

    private Bus _masterBus, _musicBus, _sfxBus, _ambientBus, _uiBus;

    private void Awake()
    {
        if (Instance) Destroy(this.gameObject);
        else
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        _ambientBus = RuntimeManager.GetBus("bus:/Ambient");
        _uiBus = RuntimeManager.GetBus("bus:/UI");
    }

    public void LoadData(SettingsData data)
    {
        MasterVolume = data.MasterVolume;
        MusicVolume = data.MusicVolume;
        SFXVolume = data.SFXVolume;
        AmbientVolume = data.AmbientVolume;
        UIVolume = data.UIVolume;
    }

    public void SaveData(SettingsData data)
    {
        data.MasterVolume = MasterVolume;
        data.MusicVolume = MusicVolume;
        data.SFXVolume = SFXVolume;
        data.AmbientVolume = AmbientVolume;
        data.UIVolume = UIVolume;
    }

    private void Update()
    {
        _masterBus.setVolume(MasterVolume);
        _musicBus.setVolume(MusicVolume);
        _sfxBus.setVolume(SFXVolume);
        _ambientBus.setVolume(AmbientVolume);
        _uiBus.setVolume(UIVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPosition, float delay = 0f)
    {
        StartCoroutine(PlayOneShotByDelay(sound, worldPosition, delay));
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    private IEnumerator PlayOneShotByDelay(EventReference sound, Vector3 worldPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;

        return emitter;
    }
}