using System.Collections.Generic;

[System.Serializable]
public class SettingsData
{
    public int Quality;
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;
    public float AmbientVolume;
    public float UIVolume;
    public bool VSync;
    public bool Particles;
    public Speed SpeedValue;
    public Temp Temperature;

    public SettingsData()
    {
        this.Quality = 2;
        this.VSync = false;
        this.Particles = true;
        this.SpeedValue = Speed.KMH;
        this.Temperature = Temp.Celsius;
        this.MasterVolume = 1;
        this.MusicVolume = 1;
        this.SFXVolume = 1;
        this.AmbientVolume = 1;
        this.UIVolume = 1;
    }

}

public enum Speed { KMH, MPH }
public enum Temp { Celsius, Fahrenheit }