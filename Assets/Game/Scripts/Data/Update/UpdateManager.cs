using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Game.Scripts.Data.Update
{
    public class UpdateManager : MonoBehaviour
    {
        [SerializeField] private string _jsonURL = "https://enjine.online/games/offroaded/version.json";
        private UpdateData data;
        private static bool _isAlreadyChecked = false;

        [System.Obsolete]
        private IEnumerator Start()
        {
            if (_isAlreadyChecked) yield break;

            UnityWebRequest request = UnityWebRequest.Get(_jsonURL);
            request.chunkedTransfer = false;
            request.disposeDownloadHandlerOnDispose = true;
            request.timeout = 60;

            yield return request.Send();

            if (request.isDone)
            {
                _isAlreadyChecked = true;
                if (request.result != UnityWebRequest.Result.Success) Debug.LogError("Failed to load update info");
                else
                {
                    data = JsonUtility.FromJson<UpdateData>(request.downloadHandler.text);
                    if (!string.IsNullOrEmpty(data.version) && !Application.version.Equals(data.version))
                        Assets.Game.Scripts.UI.UIManager.Instance.DisplayUpdatePopup(data.url);
                }
            }
        }
    }
}
