using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class MapSelector : MonoBehaviour
    {
        [SerializeField] private Transform _propertyField;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private List<MapData> _maps;
        [SerializeField] private Button _playButton;

        private int _selectedID;


        private void OnEnable()
        {
            SelectMap(0);
        }

        public void SelectMap(int id)
        {
            _selectedID = id;
            //StartCoroutine(_scroll.FocusAtPointCoroutine(_maps[id].transform.localPosition, 1.5f));
            StartCoroutine(_scroll.FocusOnItemCoroutine(_maps[id].GetComponent<RectTransform>(), 1.5f));

            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _maps[id].Config.Name;
            _propertyField.Find("Stat").Find("Lenght").GetComponent<TextMeshProUGUI>().text = "Lenght: " + _maps[id].Config.Lenght;
            _propertyField.Find("Stat").Find("Flatness").GetComponent<TextMeshProUGUI>().text = "Flatness: " + _maps[id].Config.Flatness.ToString();
            _propertyField.Find("Stat").Find("Description").GetComponent<TextMeshProUGUI>().text = _maps[id].Config.Description;

            int mapID = _maps[id].Config.SceneID;

            _playButton.onClick.RemoveAllListeners();
            _playButton.onClick.AddListener(() => UIManager.Instance.Play(mapID));

        }
    }
}