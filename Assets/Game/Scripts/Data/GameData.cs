using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public string CurrentCar;
    public int CarCount;
    public int Coins;
    public SerializableDictionary<string, Upgrades> CarUpgrades;
    public SerializableDictionary<string, bool> CarsOpened;
    public SerializableDictionary<string, string> CarsColors;
    public int CurrentTires;
    public int CurrentRims;
    public List<int> OpenedTires;
    public List<int> OpenedRims;

    public GameData()
    {
        this.CurrentCar = "7042cd7a-5792-4ce9-a3d4-41851cbc94c8";
        this.CarCount = 1;
        this.Coins = 500000;
        this.CarUpgrades = new();
        this.CarUpgrades.Add(this.CurrentCar, new Upgrades());
        this.CarsOpened = new();
        this.CarsOpened.Add(this.CurrentCar, true);
        this.CarsColors = new();
        this.CarsColors.Add(this.CurrentCar, "FFFFFFFF");
        this.CurrentTires = 0;
        this.CurrentRims = 0;
        this.OpenedTires = new List<int> { 0 };
        this.OpenedRims = new List<int> { 0 };
    }
}