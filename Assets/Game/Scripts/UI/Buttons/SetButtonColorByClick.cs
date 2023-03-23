using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetButtonColorByClick : MonoBehaviour
{
    [SerializeField] private Color _clickedColor = new(178, 178, 178, 255);
    [SerializeField] private Color _normalColor = Color.white;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
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