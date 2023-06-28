using System;
using System.Net.Mime;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;

    [SerializeField, StatusIcon] private GameObject _loaderPanel;
    [SerializeField, StatusIcon] private Animator _loaderAnimator;
    [SerializeField, StatusIcon] private UnityEngine.UI.Image _loadingIndicator;

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        else
        {
            Instance = this;
            if (transform.parent != null)
                transform.parent = null;
            Application.targetFrameRate = 120; // -1
            //QualitySettings.antiAliasing 0 2 4 8
            SceneManager.sceneUnloaded += SceneUnloaded;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void SceneUnloaded(Scene arg0) => Debug.Clear();
    

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

        currentScene.allowSceneActivation = true;
        _loaderAnimator.SetBool("loading", false);
        await Task.Delay(500);
        _loaderPanel.SetActive(false);
    }
}