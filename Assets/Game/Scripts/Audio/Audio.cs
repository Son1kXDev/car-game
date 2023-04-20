using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Audio : MonoBehaviour
{
    public static Audio Data;

    public EventReference Rain => _rain;
    public EventReference Change => _change;
    public EventReference LightSwitch => _lightSwitch;
    public EventReference GearSwitch => _gearSwitch;
    public EventReference Purchase => _purchase;
    public EventReference Spray => _spray;
    public EventReference Engine => _engine;
    public EventReference ButtonClick => _buttonClick;
    public EventReference ButtonNoClick => _buttonNoClick;

    [SerializeField] private EventReference _rain;
    [SerializeField] private EventReference _change;
    [SerializeField] private EventReference _lightSwitch;
    [SerializeField] private EventReference _gearSwitch;
    [SerializeField] private EventReference _purchase;
    [SerializeField] private EventReference _spray;
    [SerializeField] private EventReference _engine;
    [SerializeField] private EventReference _buttonClick;
    [SerializeField] private EventReference _buttonNoClick;

    void Awake()
    {
        if (Data) Destroy(this);
        else Data = this;

        this.transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }
}
