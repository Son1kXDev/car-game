using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField] private Button _selectedButton;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _buyButton;

    private Button _currentButton;

    public void ButtonSetActive(ActionButtonType type)
    {
        _selectedButton.gameObject.SetActive(false);
        _applyButton.gameObject.SetActive(false);
        _buyButton.gameObject.SetActive(false);
        switch (type)
        {
            case ActionButtonType.Selected:
                _selectedButton.interactable = false;
                _selectedButton.GetComponent<Image>().color = new(255, 255, 255, 0);
                _selectedButton.gameObject.SetActive(true);
                _currentButton = _selectedButton;
                break;
            case ActionButtonType.Apply:
                _applyButton.interactable = true;
                _applyButton.GetComponent<Image>().color = new(255, 255, 255, 255);
                _applyButton.gameObject.SetActive(true);
                _currentButton = _applyButton;
                break;
            case ActionButtonType.Buy:
                _buyButton.interactable = true;
                _buyButton.GetComponent<Image>().color = new(255, 255, 255, 255);
                _buyButton.gameObject.SetActive(true);
                _currentButton = _buyButton;
                break;
        }
    }

    public void UpdateAction(UnityEngine.Events.UnityAction call) => _currentButton.onClick.AddListener(call);

}

public enum ActionButtonType { Selected, Apply, Buy }
