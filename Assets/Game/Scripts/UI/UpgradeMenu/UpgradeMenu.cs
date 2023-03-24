using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> _panels;
    [SerializeField] private Vector2 _leftOffset;
    [SerializeField] private Vector2 _rightOffset;
    [SerializeField] private List<TMP_Text> _menuButtons;
    [SerializeField] private ScrollRect _scroll;
    private int _currentElement = 0;

    public void MoveTo(int position)
    {
        StopAllCoroutines();
        _menuButtons.ForEach(text => text.fontStyle = FontStyles.Normal);
        _menuButtons[position].fontStyle = FontStyles.Bold;
        _currentElement = position;
        StartCoroutine(OpenPanel(position));
    }

    private IEnumerator OpenPanel(int position)
    {
        yield return StartCoroutine(_scroll.FocusAtPointCoroutine(_menuButtons[position].transform.localPosition, 1.5f));
        _panels.ForEach(panel => panel.SetActive(false));
        _panels[position].SetActive(true);
    }

    public void MoveLeft()
    {
        _currentElement--;
        _currentElement = Mathf.Clamp(_currentElement, 0, _menuButtons.Count - 1);
        MoveTo(_currentElement);
    }

    public void MoveRight()
    {
        _currentElement++;
        _currentElement = Mathf.Clamp(_currentElement, 0, _menuButtons.Count - 1);
        MoveTo(_currentElement);
    }
}