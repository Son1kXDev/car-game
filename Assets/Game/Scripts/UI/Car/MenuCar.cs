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
        [SerializeField] private SpriteRenderer _body;
        [SerializeField] private SpriteRenderer _back;
        [SerializeField] private SpriteRenderer _elements;
        [SerializeField] private SpriteRenderer _optics;
        [SerializeField] private List<SpriteRenderer> _tires;
        [SerializeField] private List<SpriteRenderer> _rims;

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

            _body.sprite = _config.VisualCarConfig.BaseSprite;
            _back.sprite = _config.VisualCarConfig.BackSprite;
            _elements.sprite = _config.VisualCarConfig.ElementsSprite;
            _optics.sprite = _config.VisualCarConfig.OpticsSprite;
            _body.color = _config.CurrentColor;
            _tires.ForEach(tire => tire.sprite = _config.VisualCarConfig.TiresSprites[_config.CurrentTire]);
            _rims.ForEach(rim => rim.sprite = _config.VisualCarConfig.RimsSprites[_config.CurrentRim]);
        }

        public void ChangeCarColor(Color color) => _body.color = color;

        public void ApplyColor()
        {
            _config.CurrentColor = _body.color;
            Data.DataPersistenceManager.Instance.SaveGame();
        }

        public void ResetColor() => _body.color = _config.CurrentColor;

        public Color GetCurrentColor() => _body.color;

        public void SetRim(int value)
        {
            _config.CurrentRim = value;
            ApplyData();
        }

        public void SetTire(int value)
        {
            _config.CurrentTire = value;
            ApplyData();
        }
    }
}