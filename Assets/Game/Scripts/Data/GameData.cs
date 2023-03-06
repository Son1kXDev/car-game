using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int CurrentCar;
    public int CarCount;
    public int Coins;
    public Upgrades CurrentCarUpgrades;
    public SerializableDictionary<string, bool> carsOpened;

    public GameData()
    {
        this.CurrentCar = 0;
        this.CarCount = 1;
        this.Coins = 500;
        this.CurrentCarUpgrades = new();
        carsOpened = new();
    }
}