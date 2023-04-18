using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Volume")]
    [Range(0, 1)]
    public float MasterVolume = 1f;
    [Range(0, 1)]
    public float MusicVolume = 1f;
    [Range(0, 1)]
    public float SFXVolume = 1f;

    private Bus _masterBus, _musicBus, _sfxBus;

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
    }

    private void Update()
    {
        _masterBus.setVolume(MasterVolume);
        _musicBus.setVolume(MusicVolume);
        _sfxBus.setVolume(SFXVolume);
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
}