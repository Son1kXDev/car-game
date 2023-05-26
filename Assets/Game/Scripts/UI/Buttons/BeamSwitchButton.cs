using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Game.Scripts.Game;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(EventTrigger))]
    public class BeamSwitchButton : MonoBehaviour
    {
        [SerializeField] private Sprite _disabledBeam;
        [SerializeField] private Sprite _lowBeam;
        [SerializeField] private Sprite _highBeam;

        private CameraController _cameraController;
        private Image _image;
        private CarVisual _car;
        private EventTrigger _trigger;

        private void Start()
        {
            _image = GetComponent<Image>();
            _car = FindObjectOfType<CarVisual>();
            _cameraController = GameObject.Find("[GAME] Camera").GetComponent<CameraController>();

            _trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((eventData) => { GlobalEventManager.Instance.LightButton(false); });
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((eventData) => { GlobalEventManager.Instance.LightButton(true); });
            _trigger.triggers.Add(pointerUp);
            _trigger.triggers.Add(pointerDown);

            GlobalEventManager.Instance.OnLightButtonPressed += Switch;
        }

        private void OnDestroy()
        { GlobalEventManager.Instance.OnLightButtonPressed -= Switch; }

        public void Switch(bool value)
        {
            if (!value) return;
            _car.SwitchBeam(value);
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