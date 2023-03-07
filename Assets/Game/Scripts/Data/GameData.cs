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

    public GameData()
    {
        this.CurrentCar = "7042cd7a-5792-4ce9-a3d4-41851cbc94c8";
        this.CarCount = 1;
        this.Coins = 500;
        this.CurrentCarUpgrades = new();
        carsOpened = new();
    }
}