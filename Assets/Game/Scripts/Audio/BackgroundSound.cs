using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class BackgroundSound : MonoBehaviour
{
    [SerializeField] List<EventReference> _references;

    List<EventInstance> _events;

    void Awake()
    {
        _events = new List<EventInstance>();
        foreach (var reference in _references)
        {
            _events.Add(AudioManager.Instance.CreateEventInstance(reference));
            _events[_events.Count - 1].start();
        }
    }

    void OnDestroy()
    {
        foreach (var eventInstance in _events)
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
