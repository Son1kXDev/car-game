using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Debugger;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;

    [SerializeField] private GameObject _loaderPanel;
    [SerializeField] private UnityEngine.UI.Image _loadingIndicator;

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public enum Scenes
    {
        MainMenu, Game
    }

    public async void LoadScene(Scenes scene, bool doNotUnloadCurrentScene = false)
    {
        LoadSceneMode mode = doNotUnloadCurrentScene ? LoadSceneMode.Additive : LoadSceneMode.Single;

        int ID;

        switch (scene)
        {
            case Scenes.MainMenu:
                ID = 0;
                break;

            case Scenes.Game:
                ID = 1;
                break;

            default:
                ID = 0;
                break;
        }
        AsyncOperation currentScene = SceneManager.LoadSceneAsync(ID, mode);
        currentScene.allowSceneActivation = false;

        _loaderPanel.SetActive(true);
        _loadingIndicator.fillAmount = 0;
        do
        {
            await Task.Delay(100);
            _loadingIndicator.fillAmount = currentScene.progress;
        } while (currentScene.progress < 0.9f);

        Console.Log($"Scene {SceneManager.GetSceneAt(ID).name} is loaded", DColor.white, DType.bold);

        currentScene.allowSceneActivation = true;
        _loaderPanel.SetActive(false);
    }

    public async void LoadScene(int ID, bool doNotUnloadCurrentScene = false)
    {
        LoadSceneMode mode = doNotUnloadCurrentScene ? LoadSceneMode.Additive : LoadSceneMode.Single;

        AsyncOperation currentScene = SceneManager.LoadSceneAsync(ID, mode);
        currentScene.allowSceneActivation = false;
        _loaderPanel.SetActive(true);
        _loadingIndicator.fillAmount = 0;
        do
        {
            await Task.Delay(100);
            _loadingIndicator.fillAmount = currentScene.progress;
        } while (currentScene.progress < 0.9f);

        Console.Log($"Scene {SceneManager.GetSceneAt(ID).name} is loaded", DColor.white, DType.bold);

        currentScene.allowSceneActivation = true;
        _loaderPanel.SetActive(false);
    }
}