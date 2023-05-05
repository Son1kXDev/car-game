using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.UI
{
    public class CarInputUI : MonoBehaviour
    {
        [SerializeField] List<GameObject> _mobileInterface;

        private void Start() => _mobileInterface.ForEach(o => o.gameObject.SetActive(Application.platform == RuntimePlatform.Android));
    }
}
