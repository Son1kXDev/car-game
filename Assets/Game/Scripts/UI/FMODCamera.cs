using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODCamera : MonoBehaviour
{
    private StudioListener _listener;

    private void Awake()
    {
        GameObject attenuationObject = GameObject.Find("Car");
        _listener = GetComponent<StudioListener>();
        _listener.attenuationObject = attenuationObject;
    }
}
