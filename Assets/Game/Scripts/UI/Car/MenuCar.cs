using Assets.Game.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
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
        [SerializeField] private List<SpriteRenderer> _spoilers;
        [SerializeField] private List<SpriteRenderer> _splitters;

        [Header("Audio")]
        [SerializeField] private EventReference _spraySound;
        [SerializeField] private EventReference _сhangeSound;

        private WheelJoint2D[] _wheelJoints;

        private CarConfig _config;
        private CarInfo _carInfo;

        private void Start()
        {
            _config = GetComponent<CarConfig>();
            _carInfo = FindObjectOfType<CarInfo>(true);
            _wheelJoints = GetComponents<WheelJoint2D>();
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
            _spoilers.ForEach(spoiler => spoiler.sprite = _config.VisualCarConfig.SpoilersSprites[_config.CurrentSpoiler]);
            _splitters.ForEach(splitter => splitter.sprite = _config.VisualCarConfig.SplittersSprites[_config.CurrentSplitter]);
            foreach (WheelJoint2D wheel in _wheelJoints)
            {
                JointSuspension2D susp = wheel.suspension;
                susp.frequency = _config.MainCarConfig.DefaultSuspensionFrequency * _config.CurrentCarUpgrades.SuspensionFrequencyMultiplier;
                Vector2 anch = wheel.anchor;
                anch.y = _config.MainCarConfig.DefaultSuspensionHeight * (0.9f + _config.CurrentCarUpgrades.SuspensionHeightMultiplier / 10);
                wheel.anchor = anch;
                wheel.suspension = susp;
            }
        }

        public void UpdateSuspensionData(float frequency, float height)
        {
            foreach (WheelJoint2D wheel in _wheelJoints)
            {
                JointSuspension2D susp = wheel.suspension;
                susp.frequency = _config.MainCarConfig.DefaultSuspensionFrequency * frequency;
                Vector2 anch = wheel.anchor;
                anch.y = _config.MainCarConfig.DefaultSuspensionHeight * (0.9f + height / 10);
                wheel.anchor = anch;
                wheel.suspension = susp;
            }
        }

        public void ChangeCarColor(Color color) => _body.color = color;

        public void ApplyColor(ColourPickerControl control)
        {
            if (CoinManager.Instance.DecreaseCoins(100))
            {
                control.ApplyButton(false);
                AudioManager.Instance.PlayOneShot(_spraySound, transform.position, 0.6f);
                _config.CurrentColor = _body.color;
                if (_config.CostsDictionary.ContainsKey("Color"))
                    _config.CostsDictionary.Remove("Color");
                _config.CostsDictionary.Add("Color", 100);
                _config.Costs = new(_config.CostsDictionary.Values);
                _carInfo.UpdateDisplayData();
                Data.DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ResetCar()
        {
            _body.color = _config.CurrentColor;
            _tires.ForEach(tire => tire.sprite = _config.VisualCarConfig.TiresSprites[_config.CurrentTire]);
            _rims.ForEach(rim => rim.sprite = _config.VisualCarConfig.RimsSprites[_config.CurrentRim]);
            _spoilers.ForEach(spoiler => spoiler.sprite = _config.VisualCarConfig.SpoilersSprites[_config.CurrentSpoiler]);
            _splitters.ForEach(splitter => splitter.sprite = _config.VisualCarConfig.SplittersSprites[_config.CurrentSplitter]);
            foreach (WheelJoint2D wheel in _wheelJoints)
            {
                JointSuspension2D susp = wheel.suspension;
                susp.frequency = _config.MainCarConfig.DefaultSuspensionFrequency * _config.CurrentCarUpgrades.SuspensionFrequencyMultiplier;
                Vector2 anch = wheel.anchor;
                anch.y = _config.MainCarConfig.DefaultSuspensionHeight * (0.9f + _config.CurrentCarUpgrades.SuspensionHeightMultiplier / 10);
                wheel.anchor = anch;
                wheel.suspension = susp;
            }
        }

        public Color GetCurrentColor() => _body.color;

        public void SelectRim(int value) => _rims.ForEach(rim => rim.sprite = _config.VisualCarConfig.RimsSprites[value]);
        public void SelectTire(int value) => _tires.ForEach(tire => tire.sprite = _config.VisualCarConfig.TiresSprites[value]);
        public void SelectSpoiler(int value) => _spoilers.ForEach(spoiler => spoiler.sprite = _config.VisualCarConfig.SpoilersSprites[value]);
        public void SelectSplitter(int value) => _splitters.ForEach(splitter => splitter.sprite = _config.VisualCarConfig.SplittersSprites[value]);

        public void SetRim(int value)
        {
            _config.CurrentRim = value;
            AudioManager.Instance.PlayOneShot(_сhangeSound, transform.position);
            ApplyData();
        }

        public void SetTire(int value)
        {
            _config.CurrentTire = value;
            AudioManager.Instance.PlayOneShot(_сhangeSound, transform.position);
            ApplyData();
        }

        public void SetSpoiler(int value)
        {
            _config.CurrentSpoiler = value;
            AudioManager.Instance.PlayOneShot(_сhangeSound, transform.position);
            ApplyData();
        }

        public void SetSplitter(int value)
        {
            _config.CurrentSplitter = value;
            AudioManager.Instance.PlayOneShot(_сhangeSound, transform.position);
            ApplyData();
        }
    }
}