using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Game.Scripts.UI
{
    public class MapData : MonoBehaviour
    {
        [StatusIcon(offset: 20)] public MapConfig Config;

        private TextMeshProUGUI _name;
        private Image _image;

        void Awake()
        {
            _image = transform.Find("Image").GetComponent<Image>();
            _name = transform.Find("Name").GetComponent<TextMeshProUGUI>();

            _image.sprite = Config.Image;
            _name.text = Config.Name;
        }
    }
}
