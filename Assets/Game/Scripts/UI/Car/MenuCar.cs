using Assets.Game.Scripts.Game;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(CarConfig))]
    public class MenuCar : MonoBehaviour
    {
        [Header("Scene Visual")]
        [SerializeField, StatusIcon] private SpriteRenderer _body;
        [SerializeField, StatusIcon] private SpriteRenderer _back;
        [SerializeField, StatusIcon] private SpriteRenderer _elements;
        [SerializeField, StatusIcon] private SpriteRenderer _optics;
        [SerializeField, StatusIcon] private SpriteRenderer _sticker;
        [SerializeField, StatusIcon] private SpriteRenderer _spoiler;
        [SerializeField, StatusIcon] private SpriteRenderer _splitter;
        [SerializeField] private List<SpriteRenderer> _tires;
        [SerializeField] private List<SpriteRenderer> _rims;

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
                Debug.LogError("Config files were not found. Please check if the files are set correctly or define them manually");
                return;
            }

            _body.sprite = _config.CurrentCar.BaseSprite;
            _back.sprite = _config.CurrentCar.BackSprite;
            _elements.sprite = _config.CurrentCar.ElementsSprite;
            _optics.sprite = _config.CurrentCar.OpticsSprite;
            _body.color = _config.CurrentColor;
            _tires.ForEach(tire => tire.sprite = _config.CurrentCar.TiresSprites[_config.CurrentTire]);
            _rims.ForEach(rim => rim.sprite = _config.CurrentCar.RimsSprites[_config.CurrentRim]);
            _spoiler.sprite = _config.CurrentCar.SpoilersSprites[_config.CurrentSpoiler];
            _splitter.sprite = _config.CurrentCar.SplittersSprites[_config.CurrentSplitter];
            ChangeCarSticker(StickerUploader.GetSprite(_config.CurrentStickerPath));
            foreach (WheelJoint2D wheel in _wheelJoints)
            {
                JointSuspension2D susp = wheel.suspension;
                susp.frequency = _config.CurrentCar.SuspensionFrequency * _config.CurrentCarUpgrades.SuspensionFrequencyMultiplier;
                Vector2 anch = wheel.anchor;
                anch.y = _config.CurrentCar.SuspensionHeight * (0.9f + _config.CurrentCarUpgrades.SuspensionHeightMultiplier / 10);
                wheel.anchor = anch;
                wheel.suspension = susp;
            }
        }

        public void UpdateSuspensionData(float frequency, float height)
        {
            foreach (WheelJoint2D wheel in _wheelJoints)
            {
                JointSuspension2D susp = wheel.suspension;
                susp.frequency = _config.CurrentCar.SuspensionFrequency * frequency;
                Vector2 anch = wheel.anchor;
                anch.y = _config.CurrentCar.SuspensionHeight * (0.9f + height / 10);
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
                AudioManager.Instance.PlayOneShot(Audio.Data.Spray, transform.position, 0.6f);
                _config.CurrentColor = _body.color;
                if (_config.CostsDictionary.ContainsKey("Color"))
                    _config.CostsDictionary.Remove("Color");
                _config.CostsDictionary.Add("Color", 100);
                _config.Costs = new(_config.CostsDictionary.Values);
                _carInfo.UpdateDisplayData();
                Data.DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ChangeCarSticker(Sprite sprite)
        {
            _sticker.sprite = sprite;
        }

        public void ApplySticker(StickerUploader uploader)
        {
            if (CoinManager.Instance.DecreaseCoins(100))
            {
                uploader.ApplyButton(false);
                AudioManager.Instance.PlayOneShot(Audio.Data.Spray, transform.position, 0.6f);
                _config.CurrentStickerPath = FileManager.Instance.LocalPath;
                if (_config.CostsDictionary.ContainsKey("Sticker"))
                    _config.CostsDictionary.Remove("Sticker");
                _config.CostsDictionary.Add("Sticker", 100);
                _config.Costs = new(_config.CostsDictionary.Values);
                _carInfo.UpdateDisplayData();
                Data.DataPersistenceManager.Instance.SaveGame();
            }
        }

        public void ResetCar()
        {
            _body.color = _config.CurrentColor;
            _tires.ForEach(tire => tire.sprite = _config.CurrentCar.TiresSprites[_config.CurrentTire]);
            _rims.ForEach(rim => rim.sprite = _config.CurrentCar.RimsSprites[_config.CurrentRim]);
            _spoiler.sprite = _config.CurrentCar.SpoilersSprites[_config.CurrentSpoiler];
            _splitter.sprite = _config.CurrentCar.SplittersSprites[_config.CurrentSplitter];
            ChangeCarSticker(StickerUploader.GetSprite(_config.CurrentStickerPath));
            foreach (WheelJoint2D wheel in _wheelJoints)
            {
                JointSuspension2D susp = wheel.suspension;
                susp.frequency = _config.CurrentCar.SuspensionFrequency * _config.CurrentCarUpgrades.SuspensionFrequencyMultiplier;
                Vector2 anch = wheel.anchor;
                anch.y = _config.CurrentCar.SuspensionHeight * (0.9f + _config.CurrentCarUpgrades.SuspensionHeightMultiplier / 10);
                wheel.anchor = anch;
                wheel.suspension = susp;
            }
        }

        public Color GetCurrentColor() => _body.color;

        public void SelectRim(int value) => _rims.ForEach(rim => rim.sprite = _config.CurrentCar.RimsSprites[value]);
        public void SelectTire(int value) => _tires.ForEach(tire => tire.sprite = _config.CurrentCar.TiresSprites[value]);
        public void SelectSpoiler(int value) => _spoiler.sprite = _config.CurrentCar.SpoilersSprites[value];
        public void SelectSplitter(int value) => _splitter.sprite = _config.CurrentCar.SplittersSprites[value];

        public void SetRim(int value)
        {
            _config.CurrentRim = value;
            AudioManager.Instance.PlayOneShot(Audio.Data.Change, transform.position);
            ApplyData();
        }

        public void SetTire(int value)
        {
            _config.CurrentTire = value;
            AudioManager.Instance.PlayOneShot(Audio.Data.Change, transform.position);
            ApplyData();
        }

        public void SetSpoiler(int value)
        {
            _config.CurrentSpoiler = value;
            AudioManager.Instance.PlayOneShot(Audio.Data.Change, transform.position);
            ApplyData();
        }

        public void SetSplitter(int value)
        {
            _config.CurrentSplitter = value;
            AudioManager.Instance.PlayOneShot(Audio.Data.Change, transform.position);
            ApplyData();
        }
    }
}