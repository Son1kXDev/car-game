using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Debugger;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;

    [SerializeField] private GameObject _loaderPanel;
    [SerializeField] private Animator _loaderAnimator;
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

    public async void LoadScene(string sceneName, bool doNotUnloadCurrentScene = false)
    {
        LoadSceneMode mode = doNotUnloadCurrentScene ? LoadSceneMode.Additive : LoadSceneMode.Single;

        AsyncOperation currentScene = SceneManager.LoadSceneAsync(sceneName, mode);
        currentScene.allowSceneActivation = false;

        _loaderPanel.SetActive(true);
        _loaderAnimator.SetBool("loading", true);
        _loadingIndicator.fillAmount = 0;
        do
        {
            await Task.Delay(1000);
            _loadingIndicator.fillAmount = currentScene.progress;
        } while (currentScene.progress < 0.9f);

        Console.Log($"Scene {sceneName} is loaded", DColor.white, DType.bold);

        currentScene.allowSceneActivation = true;
        _loaderAnimator.SetBool("loading", false);
        await Task.Delay(500);
        _loaderPanel.SetActive(false);
    }

    public async void LoadScene(int ID, bool doNotUnloadCurrentScene = false)
    {
        LoadSceneMode mode = doNotUnloadCurrentScene ? LoadSceneMode.Additive : LoadSceneMode.Single;

        AsyncOperation currentScene = SceneManager.LoadSceneAsync(ID, mode);
        currentScene.allowSceneActivation = false;

        _loaderPanel.SetActive(true);
        _loaderAnimator.SetBool("loading", true);
        _loadingIndicator.fillAmount = 0;
        do
        {
            await Task.Delay(1000);
            _loadingIndicator.fillAmount = currentScene.progress;
        } while (currentScene.progress < 0.9f);

        Console.Log($"Scene {SceneManager.GetSceneAt(ID).name} is loaded", DColor.white, DType.bold);

        currentScene.allowSceneActivation = true;
        _loaderAnimator.SetBool("loading", false);
        await Task.Delay(500);
        _loaderPanel.SetActive(false);
    }
}