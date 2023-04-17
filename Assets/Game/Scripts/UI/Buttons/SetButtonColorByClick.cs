using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger), typeof(Image))]
public class SetButtonColorByClick : MonoBehaviour
{
    [SerializeField] private Color _clickedColor = new(178, 178, 178, 255);
    [SerializeField] private Color _normalColor = Color.white;

    private Image _image;
    private EventTrigger _trigger;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((eventData) => { OnPointUp(); });
        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((eventData) => { OnPointDown(); });
        _trigger.triggers.Add(pointerUp);
        _trigger.triggers.Add(pointerDown);
    }

    public void OnPointUp()
    {
        _image.color = _normalColor;
    }

    public void OnPointDown()
    {
        _image.color = _clickedColor;
    }
}