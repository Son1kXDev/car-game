using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Game.Scripts.UI
{
    [RequireComponent(typeof(EventTrigger))]
    public class BrakeButton : MonoBehaviour
    {
        private EventTrigger _trigger;
        private void Start()
        {
            _trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((eventData) => { GlobalEventManager.Instance.BrakeButton(false); });
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((eventData) => { GlobalEventManager.Instance.BrakeButton(true); });
            _trigger.triggers.Add(pointerUp);
            _trigger.triggers.Add(pointerDown);
        }
    }
}
