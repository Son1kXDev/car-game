using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Game.Scripts
{
    public class SVImageControl : MonoBehaviour
    {
        private RawImage SVImage;

        private ColourPickerControl ColourPickerControl;

        private RectTransform rectTransform, pickerTransform;

        private void Awake()
        {
            SVImage = GetComponent<RawImage>();
            ColourPickerControl = FindObjectOfType<ColourPickerControl>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void UpdateColor(PointerEventData eventData)
        {
            Vector3 pos = rectTransform.InverseTransformDirection(eventData.position);
            Debug.Log(pos);
            pos.z = 0;

            float deltaX = rectTransform.sizeDelta.x * 0.5f;
            float deltaY = rectTransform.sizeDelta.y * 0.5f;

            if (pos.x < -deltaX) pos.x = -deltaX;
            else if (pos.x > deltaX) pos.x = deltaX;

            if (pos.y < -deltaY) pos.y = -deltaY;
            else if (pos.y > deltaY) pos.y = deltaY;

            float x = pos.x + deltaX;
            float y = pos.y + deltaY;

            float xNorm = x / rectTransform.sizeDelta.x;
            float yNorm = y / rectTransform.sizeDelta.y;

            pickerTransform.localPosition = pos;
        }
    }
}