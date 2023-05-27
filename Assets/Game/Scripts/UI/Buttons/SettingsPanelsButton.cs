using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(Image))]
    public class SettingsPanelsButton : MonoBehaviour
    {
        private Image _image;

        private void Awake() => _image = GetComponent<Image>();


        public void SetImageTransparency(float value) => _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, value / 100);

    }
}
