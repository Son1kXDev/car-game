using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Custom;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class MapSelector : MonoBehaviour
    {
        [SerializeField] private Transform _propertyField;
        [SerializeField] private UnityEngine.Localization.LocalizedString _length;
        [SerializeField] private UnityEngine.Localization.LocalizedString _flatness;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private List<MapData> _maps;
        [SerializeField] private Button _playButton;
        private int _selectedID;

        private void OnEnable()
        {
            _length.Arguments = new object[] { _maps[0].Config.Length.ToString() };
            _flatness.Arguments = new object[] { _maps[0].Config.Flatness.ToString() };
            _length.StringChanged += UpdateLength;
            _flatness.StringChanged += UpdateFlatness;
            StartCoroutine(EnableSelect());
        }

        private IEnumerator EnableSelect()
        {
            yield return new WaitForSeconds(0.1f);
            SelectMap(0);
        }

        private void OnDisable()
        {
            _length.StringChanged -= UpdateLength;
            _flatness.StringChanged -= UpdateFlatness;
        }

        private void UpdateLength(string value) => _propertyField.Find("Stat").Find("Length").GetComponent<TextMeshProUGUI>().text = value;
        private void UpdateFlatness(string value) => _propertyField.Find("Stat").Find("Flatness").GetComponent<TextMeshProUGUI>().text = value;

        public void SelectMap(int id)
        {
            _selectedID = id;
            StartCoroutine(_scroll.FocusOnItemCoroutine(_maps[id].GetComponent<RectTransform>(), 1.5f));

            _propertyField.Find("Name").GetComponent<TextMeshProUGUI>().text = _maps[id].Config.Name;
            string length = Localization.GetCurrentLanguage() == Lang.English ?
            _maps[id].Config.Length.ToString() : ((MapLengthRU)_maps[id].Config.Length).ToString();
            _length.Arguments[0] = length;
            _length.RefreshString();

            string flatness = Localization.GetCurrentLanguage() == Lang.English ?
            _maps[id].Config.Length.ToString() : ((FlatnessTypeRU)_maps[id].Config.Flatness).ToString();
            _flatness.Arguments[0] = flatness;
            _flatness.RefreshString();

            _propertyField.Find("Stat").Find("Description").GetComponent<TextMeshProUGUI>().text =
            Localization.GetCurrentLanguage() == Lang.English ?
            _maps[id].Config.Description.EN : _maps[id].Config.Description.RU;

            int mapID = _maps[id].Config.SceneID;

            _playButton.onClick.RemoveAllListeners();
            _playButton.onClick.AddListener(() => UIManager.Instance.Play(mapID));

        }
    }
}