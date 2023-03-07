using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Debugger;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(CarConfig))]
    public class MenuCar : MonoBehaviour
    {
        [Header("Scene Visual")]
        [SerializeField] private SpriteRenderer _carBase;
        [SerializeField] private SpriteRenderer _carBack;
        [SerializeField] private SpriteRenderer _carElements;
        [SerializeField] private SpriteRenderer _carOptics;

        private CarConfig _config;

        private void Start()
        {
            _config = GetComponent<CarConfig>();
            ApplyData();
        }

        private void ApplyData()
        {
            if (!_config.HasConfig())
            {
                Console.LogError("Config files were not found. Please check if the files are set correctly or define them manually");
                return;
            }
            if (!_config.RightID())
            {
                Console.LogError("The configs' ID don't match. Please check the entered data and correct inconsistencies." +
                    "\n" + _config.MainCarConfig.ID + " != " + _config.VisualCarConfig.ID);
                return;
            }

            _carBase.sprite = _config.VisualCarConfig.BaseSprite;
            _carBack.sprite = _config.VisualCarConfig.BackSprite;
            _carElements.sprite = _config.VisualCarConfig.ElementsSprite;
            _carOptics.sprite = _config.VisualCarConfig.OpticsSprite;
            _carBase.color = _config.CurrentColor;
        }
    }
}