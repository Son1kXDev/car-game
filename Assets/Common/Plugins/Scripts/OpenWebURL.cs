using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Web
{
    [RequireComponent(typeof(Button))]
    public class OpenWebURL : MonoBehaviour
    {
        public string URL = "https://";

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OpenURL);
        }

        public void OpenURL()
        {
            if (URL == null)
            {
                Debug.LogError($"URl {URL} is null");
                return;
            }

            if (string.IsNullOrEmpty(URL))
            {
                Debug.LogError($"URl {URL} is null or empty");
                return;
            }

            if (string.IsNullOrWhiteSpace(URL))
            {
                Debug.LogError($"URl {URL} is null or empty");
                return;
            }

            if (!URL.Contains("https://"))
            {
                Debug.LogError("URL must starts with https://");
                return;
            }

            try
            {
                Application.OpenURL(URL);
            }
            catch (System.Exception exception)
            {
                if (exception != null) Debug.LogException(exception);
                throw;
            }
        }
    }
}