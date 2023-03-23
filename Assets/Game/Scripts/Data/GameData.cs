using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public string CurrentCar;
    public int CarCount;
    public int Coins;
    public Upgrades CurrentCarUpgrades;
    public SerializableDictionary<string, bool> carsOpened;
    public string BaseColor;
    public int CurrentTires;
    public int CurrentRims;
    public List<int> OpenedTires;
    public List<int> OpenedRims;

    public GameData()
    {
        this.CurrentCar = "7042cd7a-5792-4ce9-a3d4-41851cbc94c8";
        this.CarCount = 1;
        this.Coins = 500;
        this.CurrentCarUpgrades = new();
        this.carsOpened = new();
        this.BaseColor = "FFFFFFFF";
        this.CurrentTires = 0;
        this.CurrentRims = 0;
        this.OpenedTires = new List<int> { 0 };
        this.OpenedRims = new List<int> { 0 };
    }
}