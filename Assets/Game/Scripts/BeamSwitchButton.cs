using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts
{
    public class BeamSwitchButton : MonoBehaviour
    {
        [SerializeField] private Sprite _disabledBeam;
        [SerializeField] private Sprite _lowBeam;
        [SerializeField] private Sprite _highBeam;

        private Image _image;
        private CarVisual _car;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _car = FindObjectOfType<CarVisual>();
        }

        public void Switch()
        {
            _image.sprite = _car.GetCurrentBeam() switch
            {
                Beam.Disabled => _disabledBeam,
                Beam.Low => _lowBeam,
                Beam.High => _highBeam,
                _ => _disabledBeam
            };
        }
    }
}