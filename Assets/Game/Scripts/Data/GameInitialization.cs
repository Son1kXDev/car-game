using UnityEngine;

public class GameInitialization : MonoBehaviour
{ void Start() => SceneLoadManager.Instance.LoadScene("MainMenuScene"); }
