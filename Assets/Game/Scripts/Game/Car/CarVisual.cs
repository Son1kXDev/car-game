using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Utils.Debugger;

namespace Assets.Game.Scripts.Game
{
    [RequireComponent(typeof(CarConfig))]
    public class CarVisual : MonoBehaviour
    {
        [Header("Sprites")]
        [SerializeField] private SpriteRenderer _carBase;
        [SerializeField] private SpriteRenderer _carBack;
        [SerializeField] private SpriteRenderer _carElements;
        [SerializeField] private SpriteRenderer _carOptics;
        [SerializeField] private SpriteRenderer _carSticker;
        [SerializeField] private List<SpriteRenderer> _tires;
        [SerializeField] private List<SpriteRenderer> _rims;
        [SerializeField] private List<SpriteRenderer> _spoilers;
        [SerializeField] private List<SpriteRenderer> _splitters;

        [Header("Lights")]
        [SerializeField] private GameObject _lowBeam;
        [SerializeField] private GameObject _highBeam;
        [SerializeField] private GameObject _backCasualLights;
        [SerializeField] private Animator _backMoveLights;
        [SerializeField] private Animator _brakeLights;

        [Header("Audio")]
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
            _carSticker.sprite = StickerUploader.GetSprite(_config.CurrentStickerPath);
            _tires.ForEach(tire => tire.sprite = _config.VisualCarConfig.TiresSprites[_config.CurrentTire]);
            _rims.ForEach(rim => rim.sprite = _config.VisualCarConfig.RimsSprites[_config.CurrentRim]);
            _spoilers.ForEach(spoiler => spoiler.sprite = _config.VisualCarConfig.SpoilersSprites[_config.CurrentSpoiler]);
            _splitters.ForEach(splitter => splitter.sprite = _config.VisualCarConfig.SplittersSprites[_config.CurrentSplitter]);
        }

        public void SetLight(float axis, bool brake)
        {
            _backCasualLights.SetActive(_currentBeam != Beam.Disabled);
            _brakeLights.SetBool("active", brake);
            _backMoveLights.SetBool("active", !brake && axis < 0);
        }

        public void SwitchBeam()
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
        {
            return _currentBeam;
        }
    }

    public enum Beam
    { Disabled, Low, High }
}