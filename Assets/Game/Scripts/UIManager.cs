using UnityEngine;
using Utils.Debugger;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance) Destroy(this);
        else Instance = this;
    }

    public void ButtonMenu()
    {
        //Todo save
        SceneLoadManager.Instance.LoadScene("MainMenuScene");
    }

    public void ButtonExit() => Application.Quit();

    public void ButtonNewgame()
    {
        //todo create new save file
        SceneLoadManager.Instance.LoadScene("GameMenuScene");
    }

    public void ButtonContinue()
    {
        //todo read data from save file
        SceneLoadManager.Instance.LoadScene("GameMenuScene");
    }

    public void ButtonSettings()
    {
        //todo openSettingsPanel
    }

    public void ButtonPlay()
    {
        //todo play button
    }

    public void ButtonAchievements()
    {
    }

    public void ButtonShop()
    {
    }

    public void ButtonUpgrades()
    {
    }

    public void ButtonInventory()
    {
    }
}