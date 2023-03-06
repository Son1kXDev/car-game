using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    [RequireComponent(typeof(CarScript))]
    public class CarVisual : MonoBehaviour
    {
        [Header("Lights")]
        [SerializeField] private GameObject _lowBeam;
        [SerializeField] private GameObject _highBeam;
        [SerializeField] private GameObject _backCasualLights;
        [SerializeField] private Animator _backMoveLights;
        [SerializeField] private Animator _brakeLights;

        [Header("Color")]
        [SerializeField] private Color _carColor = Color.white;

        private Beam _currentBeam = Beam.Disabled;

        private void Awake()
        {
            _lowBeam.SetActive(_currentBeam == Beam.Low);
            _highBeam.SetActive(_currentBeam == Beam.High);
            _backCasualLights.SetActive(true);
            transform.Find("Base").GetComponent<SpriteRenderer>().color = _carColor;
        }

        public void SetLight(float axis, bool brake)
        {
            _backCasualLights.SetActive(axis >= 0 && !brake);
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
        }

        public Beam GetCurrentBeam()
        {
            return _currentBeam;
        }
    }

    public enum Beam
    { Disabled, Low, High }
}