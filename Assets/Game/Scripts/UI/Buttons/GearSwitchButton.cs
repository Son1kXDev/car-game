using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(EventTrigger))]
    public class GearSwitchButton : MonoBehaviour
    {
        private Animator _animator;
        private float _direction = 1;
        private EventTrigger _trigger;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();

            _trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((eventData) => { GlobalEventManager.Instance.GearButton(false); });
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((eventData) => { GlobalEventManager.Instance.GearButton(true); });
            _trigger.triggers.Add(pointerUp);
            _trigger.triggers.Add(pointerDown);

            GlobalEventManager.Instance.OnGearButtonPressed += SwitchDirection;
        }

        private void OnDestroy()
        { GlobalEventManager.Instance.OnGearButtonPressed -= SwitchDirection; }

        public void SwitchDirection(bool value)
        {
            if (!value) return;
            _direction *= -1f;
            _animator.SetInteger("Direction", (int)_direction);
        }
    }
}