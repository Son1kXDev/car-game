using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Utils.Debugger;

namespace Assets.Game.Scripts.Game
{

    [RequireComponent(typeof(CarConfig))]
    public class CarVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _carBase;
        [SerializeField] private SpriteRenderer _carBack;
        [SerializeField] private SpriteRenderer _carElements;
        [SerializeField] private SpriteRenderer _carOptics;
        [SerializeField] private SpriteRenderer _carSticker;
        [SerializeField] private List<SpriteRenderer> _tires;
        [SerializeField] private List<SpriteRenderer> _rims;
        [SerializeField] private SpriteRenderer _spoiler;
        [SerializeField] private SpriteRenderer _splitter;
        [SerializeField] private GameObject _lowBeam;
        [SerializeField] private GameObject _highBeam;
        [SerializeField] private GameObject _backCasualLights;
        [SerializeField] private Animator _backMoveLights;
        [SerializeField] private Animator _brakeLights;
        [SerializeField] private EventReference _lightSwitchSound;

        private CarConfig _config;
        private Beam _currentBeam = Beam.Disabled;
        private int _currentTire;
        private int _currentRim;

        private void Awake()
        {
            _lowBeam.SetActive(_currentBeam == Beam.Low);
            _highBeam.SetActive(_currentBeam == Beam.High);
            _backCasualLights.SetActive(true);
        }

        private void Start()
        {
            _config = GetComponent<CarConfig>();
            if (!_config.HasConfig())
            {
                Utils.Debugger.Console.
                LogError("Config files were not found. Please check if the files are set correctly or define them manually");
                return;
            }

            _carBase.sprite = _config.CurrentCar.BaseSprite;
            _carBack.sprite = _config.CurrentCar.BackSprite;
            _carElements.sprite = _config.CurrentCar.ElementsSprite;
            _carOptics.sprite = _config.CurrentCar.OpticsSprite;
            _carBase.color = _config.CurrentColor;
            _carSticker.sprite = StickerUploader.GetSprite(_config.CurrentStickerPath);
            _tires.ForEach(tire => tire.sprite = _config.CurrentCar.TiresSprites[_config.CurrentTire]);
            _rims.ForEach(rim => rim.sprite = _config.CurrentCar.RimsSprites[_config.CurrentRim]);
            _spoiler.sprite = _config.CurrentCar.SpoilersSprites[_config.CurrentSpoiler];
            _splitter.sprite = _config.CurrentCar.SplittersSprites[_config.CurrentSplitter];
        }

        public void SetLight(float axis, bool brake)
        {
            _backCasualLights.SetActive(_currentBeam != Beam.Disabled);
            _brakeLights.SetBool("active", brake);
            _backMoveLights.SetBool("active", !brake && axis < 0);
        }

        public void SwitchBeam(bool value)
        {
            switch (_currentBeam)
            {
                case Beam.Disabled:
                    _currentBeam = Beam.Low;
                    break;

                case Beam.Low:
                    _currentBeam = Beam.High;
                    break;

                case Beam.High:
                    _currentBeam = Beam.Disabled;
                    break;
            }
            _lowBeam.SetActive(_currentBeam == Beam.Low);
            _highBeam.SetActive(_currentBeam == Beam.High);
            AudioManager.Instance.PlayOneShot(Audio.Data.LightSwitch, transform.position);
        }

        public Beam GetCurrentBeam()
        { return _currentBeam; }
    }

    public enum Beam
    { Disabled, Low, High }
}